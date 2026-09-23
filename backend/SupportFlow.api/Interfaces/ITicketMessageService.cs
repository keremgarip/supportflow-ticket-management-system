using SupportFlow.Api.DTOs.Messages;

namespace SupportFlow.Api.Interfaces;

public interface ITicketMessageService
{
    Task<bool> CanAccessTicketAsync(int ticketId, int userId, string role, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TicketMessageDto>> GetMessagesAsync(int ticketId, CancellationToken cancellationToken = default);
}