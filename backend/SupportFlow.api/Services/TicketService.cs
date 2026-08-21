using Microsoft.EntityFrameworkCore;
using SupportFlow.Api.Data;
using SupportFlow.Api.DTOs.Tickets;
using SupportFlow.Api.Helpers;
using SupportFlow.Api.Interfaces;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _context;

    public TicketService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<TicketListDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Tickets
            .AsNoTracking()
            .OrderByDescending(ticket => ticket.CreatedAt)
            .Select(ticket => new TicketListDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                Priority = ticket.Priority,
                CategoryId = ticket.CategoryId,
                CategoryName = ticket.Category.Name,
                CustomerId = ticket.CustomerId,
                CustomerName = ticket.Customer.FullName,
                AssignedAgentId = ticket.AssignedAgentId,
                AssignedAgentName = ticket.AssignedAgent == null
                    ? null
                    : ticket.AssignedAgent.FullName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<TicketDetailDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tickets
            .AsNoTracking()
            .Where(ticket => ticket.Id == id)
            .Select(ticket => new TicketDetailDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                Priority = ticket.Priority,
                CategoryId = ticket.CategoryId,
                CategoryName = ticket.Category.Name,
                CustomerId = ticket.CustomerId,
                CustomerName = ticket.Customer.FullName,
                CustomerEmail = ticket.Customer.Email,
                AssignedAgentId = ticket.AssignedAgentId,
                AssignedAgentName = ticket.AssignedAgent == null
                    ? null
                    : ticket.AssignedAgent.FullName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                ClosedAt = ticket.ClosedAt,
                MessageCount = ticket.Messages.Count,
                AttachmentCount = ticket.Attachments.Count
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<TicketDetailDto> CreateAsync(
        CreateTicketDto dto,
        int customerId,
        CancellationToken cancellationToken = default)
    {
        var ticket = new Ticket
        {
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            Status = TicketStatuses.Open,
            Priority = dto.Priority,
            CategoryId = dto.CategoryId,
            CustomerId = customerId,
            AssignedAgentId = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ClosedAt = null
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(ticket.Id, cancellationToken)
            ?? throw new InvalidOperationException(
                "The created ticket could not be retrieved.");
    }

    public async Task<TicketDetailDto?> UpdateAsync(
        int id,
        UpdateTicketDto dto,
        CancellationToken cancellationToken = default)
    {
        var ticket = await _context.Tickets
            .SingleOrDefaultAsync(
                ticket => ticket.Id == id,
                cancellationToken);

        if (ticket is null)
        {
            return null;
        }

        ticket.Title = dto.Title.Trim();
        ticket.Description = dto.Description.Trim();
        ticket.Priority = dto.Priority;
        ticket.CategoryId = dto.CategoryId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticket = await _context.Tickets
            .SingleOrDefaultAsync(
                ticket => ticket.Id == id,
                cancellationToken);

        if (ticket is null)
        {
            return false;
        }

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> CategoryIsAvailableAsync(
        int categoryId,
        CancellationToken cancellationToken = default)
    {
        return await _context.TicketCategories
            .AsNoTracking()
            .AnyAsync(
                category =>
                    category.Id == categoryId &&
                    category.IsActive,
                cancellationToken);
    }

    public async Task<IReadOnlyList<TicketListDto>> GetByCustomerIdAsync(
    int customerId,
    CancellationToken cancellationToken = default)
    {
        return await _context.Tickets
            .AsNoTracking()
            .Where(ticket => ticket.CustomerId == customerId)
            .OrderByDescending(ticket => ticket.CreatedAt)
            .Select(ticket => new TicketListDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                Priority = ticket.Priority,
                CategoryId = ticket.CategoryId,
                CategoryName = ticket.Category.Name,
                CustomerId = ticket.CustomerId,
                CustomerName = ticket.Customer.FullName,
                AssignedAgentId = ticket.AssignedAgentId,
                AssignedAgentName = ticket.AssignedAgent == null
                    ? null
                    : ticket.AssignedAgent.FullName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<TicketDetailDto?> GetCustomerTicketByIdAsync(
    int ticketId,
    int customerId,
    CancellationToken cancellationToken = default)
    {
        return await _context.Tickets
            .AsNoTracking()
            .Where(ticket =>
                ticket.Id == ticketId &&
                ticket.CustomerId == customerId)
            .Select(ticket => new TicketDetailDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                Priority = ticket.Priority,
                CategoryId = ticket.CategoryId,
                CategoryName = ticket.Category.Name,
                CustomerId = ticket.CustomerId,
                CustomerName = ticket.Customer.FullName,
                CustomerEmail = ticket.Customer.Email,
                AssignedAgentId = ticket.AssignedAgentId,
                AssignedAgentName = ticket.AssignedAgent == null
                    ? null
                    : ticket.AssignedAgent.FullName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                ClosedAt = ticket.ClosedAt,
                MessageCount = ticket.Messages.Count,
                AttachmentCount = ticket.Attachments.Count
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
    public async Task<TicketDetailDto?> UpdateCustomerTicketAsync(
    int ticketId,
    int customerId,
    UpdateTicketDto dto,
    CancellationToken cancellationToken = default)
    {
        var ticket = await _context.Tickets
            .SingleOrDefaultAsync(
                ticket =>
                    ticket.Id == ticketId &&
                    ticket.CustomerId == customerId,
                cancellationToken);

        if (ticket is null)
        {
            return null;
        }

        ticket.Title = dto.Title.Trim();
        ticket.Description = dto.Description.Trim();
        ticket.Priority = dto.Priority;
        ticket.CategoryId = dto.CategoryId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(
            cancellationToken);

        return await GetCustomerTicketByIdAsync(
            ticketId,
            customerId,
            cancellationToken);
    }

    public async Task<bool> DeleteCustomerTicketAsync(
        int ticketId,
        int customerId,
        CancellationToken cancellationToken = default)
    {
        var ticket = await _context.Tickets
            .SingleOrDefaultAsync(
                ticket =>
                    ticket.Id == ticketId &&
                    ticket.CustomerId == customerId,
                    cancellationToken
            );

        if (ticket is null)
        {
            return false;
        }

        _context.Tickets.Remove(ticket);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IReadOnlyList<TicketListDto>> GetByAssignedAgentIdAsync(
        int agentId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Tickets
            .AsNoTracking()
            .Where(ticket => ticket.AssignedAgentId == agentId)
            .OrderByDescending(ticket => ticket.CreatedAt)
            .Select(ticket => new TicketListDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                Priority = ticket.Priority,
                CategoryId = ticket.CategoryId,
                CategoryName = ticket.Category.Name,
                CustomerId = ticket.CustomerId,
                CustomerName = ticket.Customer.FullName,
                AssignedAgentId = ticket.AssignedAgentId,
                AssignedAgentName = ticket.AssignedAgent == null
                    ? null
                    : ticket.AssignedAgent.FullName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<TicketDetailDto?> GetAssignedAgentTicketByIdAsync(
        int ticketId,
        int agentId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Tickets
            .AsNoTracking()
            .Where(ticket =>
                ticket.Id == ticketId &&
                ticket.AssignedAgentId == agentId)
            .Select(ticket => new TicketDetailDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                Priority = ticket.Priority,
                CategoryId = ticket.CategoryId,
                CategoryName = ticket.Category.Name,
                CustomerId = ticket.CustomerId,
                CustomerName = ticket.Customer.FullName,
                CustomerEmail = ticket.Customer.Email,
                AssignedAgentId = ticket.AssignedAgentId,
                AssignedAgentName = ticket.AssignedAgent == null
                    ? null
                    : ticket.AssignedAgent.FullName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                ClosedAt = ticket.ClosedAt,
                MessageCount = ticket.Messages.Count,
                AttachmentCount = ticket.Attachments.Count
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<TicketAssignmentServiceResult> AssignAgentAsync(
        int ticketId,
        int agentId,
        CancellationToken cancellationToken = default
    )
    {
        var ticket = await _context.Tickets
            .SingleOrDefaultAsync(
                ticket => ticket.Id == ticketId,
                cancellationToken
            );

        if (ticket is null)
        {
            return new TicketAssignmentServiceResult
            {
                Status = TicketAssignmentStatus.TicketNotFound
            };
        }

        if (ticket.Status == TicketStatuses.Closed)
        {
            return new TicketAssignmentServiceResult
            {
                Status = TicketAssignmentStatus.TicketClosed
            };
        }

        var agent = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(
                user => user.Id == agentId &&
                user.IsActive &&
                user.Role == AppRoles.SupportAgent,
                cancellationToken
            );

        if (agent is null)
        {
            return new TicketAssignmentServiceResult
            {
                Status = TicketAssignmentStatus.AgentNotFoundOrInvalid
            };
        }

        if (ticket.AssignedAgentId == agentId)
        {
            return new TicketAssignmentServiceResult
            {
                Status = TicketAssignmentStatus.AlreadyAssignedToAgent
            };
        }

        var previousStatus = ticket.Status;

        ticket.AssignedAgentId = agentId;
        ticket.UpdatedAt = DateTime.UtcNow;

        if (ticket.Status == TicketStatuses.Open)
        {
            ticket.Status = TicketStatuses.InProgress;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new TicketAssignmentServiceResult
        {
            Status = TicketAssignmentStatus.Success,
            Assignment = new TicketAssignmentResultDto
            {
                TicketId = ticket.Id,
                AgentId = agent.Id,
                AgentName = agent.FullName,
                PreviousStatus = previousStatus,
                CurrentStatus = ticket.Status,
                UpdatedAt = ticket.UpdatedAt
            }
        };
    }

    public async Task<TicketStatusUpdateServiceResult> UpdateStatusAsync(int ticketId, string newStatus, int changedByUserId, CancellationToken cancellationToken = default)
    {
        var ticket = await _context.Tickets
            .SingleOrDefaultAsync(
                ticket => ticket.Id == ticketId,
                cancellationToken
            );

        if (ticket is null)
        {
            return new TicketStatusUpdateServiceResult
            {
                Status = TicketStatusUpdateStatus.TicketNotFound
            };
        }

        if (newStatus == TicketStatuses.InProgress && ticket.AssignedAgentId is null)
        {
            return new TicketStatusUpdateServiceResult
            {
                Status = TicketStatusUpdateStatus.AgentAssignmentRequired,
                CurrentStatus = ticket.Status,
                RequestedStatus = newStatus
            };
        }

        if (!TicketStatusTransitions.CanTransition(
                ticket.Status,
                newStatus
        ))
        {
            return new TicketStatusUpdateServiceResult
            {
                Status = TicketStatusUpdateStatus.InvalidTransition,
                CurrentStatus = ticket.Status,
                RequestedStatus = newStatus
            };
        }

        var previousStatus = ticket.Status;
        var updatedAt = DateTime.UtcNow;

        ticket.Status = newStatus;
        ticket.UpdatedAt = updatedAt;

        if (newStatus == TicketStatuses.Closed)
        {
            ticket.ClosedAt = updatedAt;
        }
        else
        {
            ticket.ClosedAt = null;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return new TicketStatusUpdateServiceResult
        {
            Status = TicketStatusUpdateStatus.Success,

            Result = new TicketStatusUpdateResultDto
            {
                TicketId = ticket.Id,
                PreviousStatus = previousStatus,
                currentStatus = ticket.Status,
                UpdatedAt = ticket.UpdatedAt
            }

        };
    }

    public async Task<bool> CanUserManageTicketStatusAsync(int ticketId, int userId, string role, CancellationToken cancellationToken = default)
    {
        if (role == AppRoles.Admin)
        {
            return await _context.Tickets
                .AsNoTracking()
                .AnyAsync(
                    ticket => ticket.Id == ticketId,
                    cancellationToken
                );
        }

        if (role == AppRoles.SupportAgent)
        {
            return await _context.Tickets
                .AsNoTracking()
                .AnyAsync(
                    ticket => ticket.Id == ticketId &&
                    ticket.AssignedAgentId == userId,
                    cancellationToken
                );
        }
        return false;
    }
}