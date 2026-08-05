using Microsoft.AspNetCore.Mvc;
using SupportFlow.Api.DTOs.Auth;
using SupportFlow.Api.Interfaces;

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
        typeof(RegisterResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterResponseDto>> Register(RegisterDto dto, CancellationToken cancellationToken)
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
}