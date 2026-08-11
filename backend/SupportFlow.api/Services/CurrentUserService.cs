using System.Security.Claims;
using SupportFlow.Api.Interfaces;

namespace SupportFlow.Api.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public int? UserId
    {
        get
        {
            var value = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var userId) ? userId : null;
        }
    }

    public string? FullName => User?.FindFirstValue(ClaimTypes.Name);
    public string? Email => User?.FindFirstValue(ClaimTypes.Email);
    public string? Role => User?.FindFirstValue(ClaimTypes.Role);
    public bool IsInRole(string role)
    {
        return User?.IsInRole(role) == true;
    }
}