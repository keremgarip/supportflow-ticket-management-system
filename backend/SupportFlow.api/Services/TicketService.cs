using Microsoft.EntityFrameworkCore;
using SupportFlow.Api.Data;
using SupportFlow.Api.DTOs.Tickets;
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
        CancellationToken cancellationToken = default)
    {
        var ticket = new Ticket
        {
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            Status = "Open",
            Priority = dto.Priority,
            CategoryId = dto.CategoryId,
            CustomerId = dto.CustomerId,
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

    public async Task<bool> CustomerIsValidAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(
                user =>
                    user.Id == customerId &&
                    user.IsActive &&
                    user.Role == "Customer",
                cancellationToken);
    }
}