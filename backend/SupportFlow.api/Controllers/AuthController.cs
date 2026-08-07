using Microsoft.AspNetCore.Mvc;
using SupportFlow.Api.DTOs.Auth;
using SupportFlow.Api.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SupportFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(
        typeof(AuthResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto, CancellationToken cancellationToken)
    {
        var emailExists = await _authService.EmailExistsAsync(dto.Email, cancellationToken);

        if (emailExists)
        {
            return Conflict(new
            {
                success = false,
                message = "An account with this email already exists."
            });
        }

        var result = await _authService.RegisterAsync(
            dto,
            cancellationToken);

        return Created(
            $"/api/users/{result.User.Id}",
            result);
    }

    [HttpPost("login")]
    [ProducesResponseType(
        typeof(AuthResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(dto, cancellationToken);

        if (result is null)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid email or password."
            });
        }
        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(
    typeof(AuthUserDto),
    StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthUserDto>> GetCurrentUser(
    CancellationToken cancellationToken)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var userId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "The authentication token is invalid."
            });
        }

        var user = await _authService.GetCurrentUserAsync(
            userId,
            cancellationToken);

        if (user is null || !user.IsActive)
        {
            return Unauthorized(new
            {
                success = false,
                message = "The authenticated user is no longer available."
            });
        }

        return Ok(user);
    }
}