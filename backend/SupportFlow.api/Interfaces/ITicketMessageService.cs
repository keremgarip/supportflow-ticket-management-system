namespace SupportFlow.Api.Interfaces;

public interface ITicketMessageService
{
    Task<bool> CanAccessTicketAsync(int ticketId, int userId, string role, CancellationToken cancellationToken = default);
}