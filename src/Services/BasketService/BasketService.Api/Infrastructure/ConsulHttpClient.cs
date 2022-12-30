using Consul;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Polly;
using System.Text;

namespace BasketService.Api.Infrastructure
{
 
  public interface IConsulHttpClient
  {
    Task<TResponse> GetAsync<TResponse>(string serviceName, string endpoint);
    Task<TResponse> PostAsync<TResponse, TParam>(string serviceName, string endpoint, TParam param);
  }

  // Kuzey-Güney Microservicleri birbirlerine istek attıklarında bu service vasıtası ile haberleşecekler.
  public class ConsulHttpClient : IConsulHttpClient
  {
    
    private IConsulClient _consulclient;

    public ConsulHttpClient(IConsulClient consulclient)
    {
      _consulclient = consulclient;
    }

    public async Task<T> GetAsync<T>(string serviceName, string requestUri)
    {

      var uri = await GetRequestUriAsync(serviceName, requestUri);
      T result = default(T);

      // Polly kütüphanesi Retry Pattern

      // retry policy ile istek 3 kez denesin 3 kez sonunda bir cevap alınmaz ise service erişilemediğinin hatası verilsin
      await Polly.Policy.Handle<Exception>().RetryAsync(3, (e, r) =>
       {}).ExecuteAsync(async () =>
       {
         // burada 3 kez denecek olan kod yazıldı.
         // servis kesintisi minimum seviyede etkilensin haberleşme kesintiye uğramasın diye yaptık

         using HttpClient client = new HttpClient();
         var response = await client.GetAsync(uri);

        
         var content = await response.Content.ReadAsStringAsync();
         result = JsonConvert.DeserializeObject<T>(content);

       });

      return await Task.FromResult(result);

    }

    public async Task<TResponse> PostAsync<TResponse, TParam>(string serviceName, string requestUri, TParam param)
    {


      var jsonString = JsonConvert.SerializeObject(param);
      var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

      var uri = await GetRequestUriAsync(serviceName, requestUri);


      using (HttpClient client = new HttpClient())
      {
        var response = await client.PostAsync(uri, stringContent);

        if (!response.IsSuccessStatusCode)
        {
          return default(TResponse);
        }

        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TResponse>(content);
      }

    }

    private async Task<string> GetRequestUriAsync(string serviceName, string uri)
    {
      //Get all services registered on Consul
      var allRegisteredServices = await _consulclient.Agent.Services();
      // consule kayıtlı tüm servisleri buluruz.

      //Get all instance of the service went to send a request to
      // serviceName ProductService olanı git comsul üzerinden oku.
      var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();
      // sistemde aynı serviceName isminde ama farklı serviceId farklı instance yer alabilir. o yüzdne hangi service'den sonuç döneceğine karar verecek bir mekanizma olmalı.

      //Get a random instance of the service
      var service = GetRandomInstance(registeredServices, serviceName);

      if (service == null)
      {
        throw new Exception($"Consul service: {serviceName} not found");
      }

      var url = $"http://{service.Address}:{service.Port}/{uri}";

      return url;
    }

    /// <summary>
    /// Aynı service isminde birden fazla service ayakta ise bu durumda bu serviceler içinden birini random olarak getiririz.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    private AgentService GetRandomInstance(IList<AgentService> services, string serviceName)
    {
      Random _random = new Random();

      AgentService servToUse = null;

      // ProductService 1323:5001
      // ProductService 324324:5003
      // ProductService 7567657:5004
      // 2
      servToUse = services[_random.Next(0, services.Count)];

      return servToUse;
    }


  }
}
