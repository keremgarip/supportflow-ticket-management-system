using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportFlow.Api.DTOs.Messages;
using SupportFlow.Api.Interfaces;

namespace SupportFlow.Api.Controllers;

[ApiController]
[Route("api/tickets/{ticketId:int}/messages")]
[Authorize]
public class TicketMessagesController : ControllerBase
{
    private readonly ITicketMessageService _ticketMessageService;
    private readonly ICurrentUserService _currentUserService;

    public TicketMessagesController(
        ITicketMessageService ticketMessageService,
        ICurrentUserService currentUserService)
    {
        _ticketMessageService = ticketMessageService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<TicketMessageDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<TicketMessageDto>>>
        GetMessages(
            int ticketId,
            CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue ||
            string.IsNullOrWhiteSpace(_currentUserService.Role))
        {
            return Unauthorized();
        }

        var canAccess =
            await _ticketMessageService.CanAccessTicketAsync(
                ticketId,
                _currentUserService.UserId.Value,
                _currentUserService.Role,
                cancellationToken);

        if (!canAccess)
        {
            return NotFound(new
            {
                success = false,
                message = $"Ticket with ID {ticketId} was not found."
            });
        }

        var messages =
            await _ticketMessageService.GetMessagesAsync(
                ticketId,
                cancellationToken);

        return Ok(messages);
    }
}