using Microsoft.EntityFrameworkCore;
using SupportFlow.Api.Data;
using SupportFlow.Api.Helpers;
using SupportFlow.Api.Interfaces;

namespace SupportFlow.Api.Services;

public class TicketMessageService : ITicketMessageService
{
    private readonly AppDbContext _context;

    public TicketMessageService(AppDbContext context)
    {
        _context = context;        
    }

    public async Task<bool> CanAccessTicketAsync(int ticketId, int userId, string role, CancellationToken cancellationToken = default)
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
                    ticket =>
                        ticket.Id == ticketId &&
                        ticket.AssignedAgentId == userId,
                        cancellationToken
                );
        }
        if (role == AppRoles.Customer)
        {
            return await _context.Tickets
                .AsNoTracking()
                .AnyAsync(
                    ticket =>
                        ticket.Id == ticketId &&
                        ticket.CustomerId == userId,
                        cancellationToken
                );
        }

        return false;
    }
}