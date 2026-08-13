using Microsoft.AspNetCore.Mvc;
using SupportFlow.Api.DTOs.Tickets;
using SupportFlow.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SupportFlow.Api.Helpers;

namespace SupportFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly ICurrentUserService _currentUserService;

    public TicketsController(ITicketService ticketService, ICurrentUserService currentUserService)
    {
        _ticketService = ticketService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<TicketListDto>),
        StatusCodes.Status200OK)]
    [Authorize(Roles = AppRoles.Customer)]
    [HttpGet]
    [ProducesResponseType(
    typeof(IReadOnlyList<TicketListDto>),
    StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<TicketListDto>>> GetAll(
    CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized();
        }

        var tickets =
            await _ticketService.GetByCustomerIdAsync(
                _currentUserService.UserId.Value,
                cancellationToken);

        return Ok(tickets);
    }

    [Authorize(Roles = AppRoles.Customer)]
    [HttpGet("{id:int}")]
    [ProducesResponseType(
    typeof(TicketDetailDto),
    StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TicketDetailDto>> GetById(
    int id,
    CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized();
        }

        var ticket =
            await _ticketService.GetCustomerTicketByIdAsync(
                id,
                _currentUserService.UserId.Value,
                cancellationToken);

        if (ticket is null)
        {
            return NotFound(new
            {
                success = false,
                message = $"Ticket with ID {id} was not found."
            });
        }

        return Ok(ticket);
    }

    [Authorize(Roles = AppRoles.Customer)]
    [HttpPost]
    [ProducesResponseType(
    typeof(TicketDetailDto),
    StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TicketDetailDto>> Create(
    CreateTicketDto dto,
    CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized();
        }

        var categoryIsAvailable =
            await _ticketService.CategoryIsAvailableAsync(
                dto.CategoryId,
                cancellationToken);

        if (!categoryIsAvailable)
        {
            return BadRequest(new
            {
                success = false,
                message =
                    "The selected category does not exist or is inactive."
            });
        }

        var ticket = await _ticketService.CreateAsync(
            dto,
            _currentUserService.UserId.Value,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = ticket.Id },
            ticket);
    }

    [Authorize(Roles = AppRoles.Customer)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(
    typeof(TicketDetailDto),
    StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TicketDetailDto>> Update(
    int id,
    UpdateTicketDto dto,
    CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized();
        }

        var categoryIsAvailable =
            await _ticketService.CategoryIsAvailableAsync(
                dto.CategoryId,
                cancellationToken);

        if (!categoryIsAvailable)
        {
            return BadRequest(new
            {
                success = false,
                message =
                    "The selected category does not exist or is inactive."
            });
        }

        var updatedTicket =
            await _ticketService.UpdateCustomerTicketAsync(
                id,
                _currentUserService.UserId.Value,
                dto,
                cancellationToken);

        if (updatedTicket is null)
        {
            return NotFound(new
            {
                success = false,
                message = $"Ticket with ID {id} was not found."
            });
        }

        return Ok(updatedTicket);
    }

    [Authorize(Roles = AppRoles.Customer)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(
    int id,
    CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized();
        }

        var deleted =
            await _ticketService.DeleteCustomerTicketAsync(
                id,
                _currentUserService.UserId.Value,
                cancellationToken);

        if (!deleted)
        {
            return NotFound(new
            {
                success = false,
                message = $"Ticket with ID {id} was not found."
            });
        }

        return NoContent();
    }
}