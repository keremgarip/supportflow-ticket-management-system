using Microsoft.AspNetCore.Mvc;
using SupportFlow.Api.DTOs.Tickets;
using SupportFlow.Api.Interfaces;

namespace SupportFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<TicketListDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TicketListDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var tickets = await _ticketService.GetAllAsync(
            cancellationToken);

        return Ok(tickets);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(TicketDetailDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDetailDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.GetByIdAsync(
            id,
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

    [HttpPost]
    [ProducesResponseType(
        typeof(TicketDetailDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TicketDetailDto>> Create(
        CreateTicketDto dto,
        CancellationToken cancellationToken)
    {
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

        var customerIsValid =
            await _ticketService.CustomerIsValidAsync(
                dto.CustomerId,
                cancellationToken);

        if (!customerIsValid)
        {
            return BadRequest(new
            {
                success = false,
                message =
                    "The selected customer does not exist, is inactive, " +
                    "or does not have the Customer role."
            });
        }

        var ticket = await _ticketService.CreateAsync(
            dto,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = ticket.Id },
            ticket);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(
        typeof(TicketDetailDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDetailDto>> Update(
        int id,
        UpdateTicketDto dto,
        CancellationToken cancellationToken)
    {
        var existingTicket = await _ticketService.GetByIdAsync(
            id,
            cancellationToken);

        if (existingTicket is null)
        {
            return NotFound(new
            {
                success = false,
                message = $"Ticket with ID {id} was not found."
            });
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

        var updatedTicket = await _ticketService.UpdateAsync(
            id,
            dto,
            cancellationToken);

        return Ok(updatedTicket);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _ticketService.DeleteAsync(
            id,
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