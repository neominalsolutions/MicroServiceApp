using Consul;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Text;

namespace Web.ApiGateway.Infrastructure
{
 
  public interface IConsulHttpClient
  {
    Task<TResponse> GetAsync<TResponse>(string serviceName, string requestUri);
    Task<TResponse> PostAsync<TResponse, TParam>(string serviceName, string requestUri, TParam param);
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

      using (HttpClient client = new HttpClient())
      {
        var response = await client.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
          return default(T);
        }

        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(content);
      }
     
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

      //Get all instance of the service went to send a request to
      var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();

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

      servToUse = services[_random.Next(0, services.Count)];

      return servToUse;
    }


  }
}
