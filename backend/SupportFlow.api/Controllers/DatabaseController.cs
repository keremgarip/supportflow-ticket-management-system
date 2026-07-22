using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportFlow.Api.Data;

namespace SupportFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly AppDbContext _context;

    public DatabaseController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("health")]
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
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    success = false,
                    message = "Database connection failed.",
                    error = exception.Message
                });
        }
    }
}