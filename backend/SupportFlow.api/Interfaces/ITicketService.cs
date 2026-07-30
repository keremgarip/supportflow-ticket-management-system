using SupportFlow.Api.DTOs.Tickets;

namespace SupportFlow.Api.Interfaces;

public interface ITicketService
{
    Task<IReadOnlyList<TicketListDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<TicketDetailDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<TicketDetailDto> CreateAsync(
        CreateTicketDto dto,
        CancellationToken cancellationToken = default);

    Task<TicketDetailDto?> UpdateAsync(
        int id,
        UpdateTicketDto dto,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<bool> CategoryIsAvailableAsync(
        int categoryId,
        CancellationToken cancellationToken = default);

    Task<bool> CustomerIsValidAsync(
        int customerId,
        CancellationToken cancellationToken = default);
}