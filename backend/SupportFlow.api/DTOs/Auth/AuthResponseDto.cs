namespace SupportFlow.Api.DTOs.Auth;
public class AuthResponseDto
{
    public string Token {get; set;} = string.Empty;
    public DateTime ExpiresAt {get; set;}
    public AuthUserDto User {get; set;} = new();
}