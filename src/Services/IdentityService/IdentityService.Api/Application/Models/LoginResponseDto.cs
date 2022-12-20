namespace IdentityService.Api.Application.Models
{
  public class LoginResponseDto
  {
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
  }
}
