using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportFlow.Api.Data;

namespace SupportFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public DatabaseController(
        AppDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CheckConnection(
        CancellationToken cancellationToken)
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync(
                cancellationToken);

            if (!canConnect)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    new
                    {
                        success = false,
                        message = "Database connection failed."
                    });
            }

            return Ok(new
            {
                success = true,
                message = "Database connection is successful."
            });
        }
        catch (Exception exception)
        {
            var response = new
            {
                success = false,
                message = "Database connection failed.",
                error = _environment.IsDevelopment()
                    ? exception.Message
                    : null
            };

            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                response);
        }
    }
}