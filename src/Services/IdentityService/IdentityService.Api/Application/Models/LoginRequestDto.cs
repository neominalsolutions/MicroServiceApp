namespace IdentityService.Api.Application.Models
{
  public class LoginRequestDto
  {
    public string UserName { get; set; }

    public string Password { get; set; }

    public string Roles { get; set; }

    public string[] Scopes { get; set; }
  }
}
