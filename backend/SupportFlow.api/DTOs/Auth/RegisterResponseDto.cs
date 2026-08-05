namespace SupportFlow.Api.DTOs.Auth;

public class RegisterResponseDto
{
    public string Message {get; set;} = string.Empty;
    public AuthUserDto User {get; set;} = new();
}