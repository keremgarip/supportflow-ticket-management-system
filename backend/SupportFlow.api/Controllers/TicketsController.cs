using Microsoft.AspNetCore.Mvc;
using SupportFlow.Api.DTOs.Tickets;
using SupportFlow.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SupportFlow.Api.Helpers;
using SupportFlow.Api.Models;

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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<TicketListDto>>> GetAll(
    CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized();
        }

        var userId = _currentUserService.UserId.Value;

        if (_currentUserService.IsInRole(AppRoles.Admin))
        {
            var tickets = await _ticketService.GetAllAsync(cancellationToken);

            return Ok(tickets);
        }

        if (_currentUserService.IsInRole(AppRoles.SupportAgent))
        {
            var tickets = await _ticketService.GetByAssignedAgentIdAsync(userId, cancellationToken);

            return Ok(tickets);
        }

        if (_currentUserService.IsInRole(AppRoles.Customer))
        {
            var tickets = await _ticketService.GetByCustomerIdAsync(userId, cancellationToken);

            return Ok(tickets);
        }

        return Forbid();
    }

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

        var userId = _currentUserService.UserId.Value;

        TicketDetailDto? ticket;

        if (_currentUserService.IsInRole(AppRoles.Admin))
        {
            ticket = await _ticketService.GetByIdAsync(id, cancellationToken);
        }
        else if (_currentUserService.IsInRole(AppRoles.SupportAgent))
        {
            ticket = await _ticketService.GetAssignedAgentTicketByIdAsync(id, userId, cancellationToken);
        }
        else if (_currentUserService.IsInRole(AppRoles.Customer))
        {
            ticket = await _ticketService.GetCustomerTicketByIdAsync(id, userId, cancellationToken);
        }
        else
        {
            return Forbid();
        }

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

        TicketDetailDto? updatedTicket;

        if (_currentUserService.IsInRole(AppRoles.Admin))
        {
            updatedTicket = await _ticketService.UpdateAsync(
                id,
                dto,
                cancellationToken
            );
        }
        else if (_currentUserService.IsInRole(AppRoles.Customer))
        {
            updatedTicket =
                await _ticketService.UpdateCustomerTicketAsync(
                    id,
                    _currentUserService.UserId.Value,
                    dto,
                    cancellationToken
                );
        }
        else
        {
            return Forbid();
        }

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

        bool deleted;

        if (_currentUserService.IsInRole(AppRoles.Admin))
        {
            deleted = await _ticketService.DeleteAsync(id, cancellationToken);
        }
        else if (_currentUserService.IsInRole(AppRoles.Customer))
        {
            deleted =
                await _ticketService.DeleteCustomerTicketAsync(
                    id,
                    _currentUserService.UserId.Value,
                    cancellationToken
                );
        }
        else
        {
            return Forbid();
        }

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

    [Authorize(Policy = AppPolicies.AdminOnly)]
    [HttpPut("{id:int}/assign")]
    [ProducesResponseType(
    typeof(TicketAssignmentResultDto),
    StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TicketAssignmentResultDto>> AssignAgent(
        int id,
        AssignTicketDto dto,
        CancellationToken cancellationToken
    )
    {
        var result = await _ticketService.AssignAgentAsync(id, dto.AgentId, cancellationToken);

        return result.Status switch
        {
            TicketAssignmentStatus.Success => Ok(result.Assignment),
            TicketAssignmentStatus.TicketNotFound =>
                NotFound(new
                {
                    success = false,
                    message = $"Ticket with ID {id} was not found."
                }),

            TicketAssignmentStatus.AgentNotFoundOrInvalid =>
                BadRequest(new
                {
                    success = false,
                    message = "The selected user does not exist, is inactive, " +
                                "or does not have the SupportAgent role."
                }),
            
            TicketAssignmentStatus.AlreadyAssignedToAgent =>
                Conflict(new
                {
                    success = false,
                    message = "The ticket is already assigned to this support agent."
                }),
            
            TicketAssignmentStatus.TicketClosed =>
                NotFound(new
                {
                    success = false,
                    message = "A closed ticket cannot be assigned to a support agent."
                }),
            
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [Authorize(Policy = AppPolicies.AgentOrAdmin)]
    [HttpPut("{id:int}/status")]
    [ProducesResponseType(
    typeof(TicketStatusUpdateResultDto),
    StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TicketStatusUpdateResultDto>> UpdateStatus(
        int id,
        UpdateTicketStatusDto dto,
        CancellationToken cancellationToken
    )
    {
        if(!_currentUserService.UserId.HasValue ||
            string.IsNullOrWhiteSpace(_currentUserService.Role))
        {
            return Unauthorized();
        }

        var userId = _currentUserService.UserId.Value;
        var role = _currentUserService.Role;

        var canManageStatus =
            await _ticketService.CanUserManageTicketStatusAsync(id, userId, role, cancellationToken);

        if (!canManageStatus)
        {
            return NotFound(new
            {
                success = false,
                message = $"Ticket with ID {id} was not found."
            });
        }

        var result = await _ticketService.UpdateStatusAsync(id, dto.Status, userId, cancellationToken);

        return result.Status switch
        {
            TicketStatusUpdateStatus.Success => Ok(result.Result),
            TicketStatusUpdateStatus.TicketNotFound =>
                NotFound(new
                {
                    success = false,
                    message = $"Ticket with ID {id} was not found."
                }),
            TicketStatusUpdateStatus.InvalidTransition =>
                Conflict(new
                {
                    success = false,
                    message =
                        $"Ticket status cannot transition from " +
                        $"'{result.CurrentStatus}' to " +
                        $"'{result.RequestedStatus}'."
                }),
            TicketStatusUpdateStatus.AgentAssignmentRequired =>
                Conflict(new
                {
                    success = false,
                    message = "A ticket must be assigned to a support agent " +
                    "before it can move to In Progress."
                }),
            
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}