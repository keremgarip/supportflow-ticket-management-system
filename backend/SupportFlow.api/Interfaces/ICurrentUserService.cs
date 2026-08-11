namespace SupportFlow.Api.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated {get;}
    int? UserId {get;}
    string? FullName {get;}
    string? Email {get;}
    string? Role {get;}
    bool IsInRole(string role);
}