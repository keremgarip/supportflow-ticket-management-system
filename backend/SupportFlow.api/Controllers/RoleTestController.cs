using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportFlow.Api.Helpers;
using SupportFlow.Api.Interfaces;

namespace SupportFlow.Api.Controllers;

[ApiController]
[Route("api/role-test")]
[Authorize]
public class RoleTestController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    public RoleTestController(
        ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    [HttpGet("authenticated")]
    public IActionResult Authenticated()
    {
        return Ok(new
        {
            message = "Authenticated access granted.",
            userId = _currentUserService.UserId,
            role = _currentUserService.Role
        });
    }

    [Authorize(Roles = AppRoles.Customer)]
    [HttpGet("customer")]
    public IActionResult CustomerOnly()
    {
        return Ok(new
        {
            message = "Customer access granted."
        });
    }

    [Authorize(Roles = AppRoles.SupportAgent)]
    [HttpGet("agent")]
    public IActionResult AgentOnly()
    {
        return Ok(new
        {
            message = "SupportAgent access granted."
        });
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet("admin")]
    public IActionResult AdminOnly()
    {
        return Ok(new
        {
            message = "Admin access granted."
        });
    }

    [Authorize(Policy = AppPolicies.AgentOrAdmin)]
    [HttpGet("agent-or-admin")]
    public IActionResult AgentOrAdmin()
    {
        return Ok(new
        {
            message = "Agent or Admin access granted."
        });
    }

    [Authorize(Policy = AppPolicies.AdminOnly)]
    [HttpGet("admin-policy")]
    public IActionResult AdminPolicy()
    {
        return Ok(new
        {
            message = "Admin policy access granted."
        });
    }
}