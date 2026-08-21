using SupportFlow.Api.DTOs.Tickets;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Interfaces;

public interface ITicketService
{
    Task<IReadOnlyList<TicketListDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TicketListDto>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<TicketDetailDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<TicketDetailDto?> GetCustomerTicketByIdAsync(
        int ticketId,
        int customerId,
        CancellationToken cancellationToken = default);

    Task<TicketDetailDto> CreateAsync(
        CreateTicketDto dto,
        int customerId,
        CancellationToken cancellationToken = default);

    Task<TicketDetailDto?> UpdateCustomerTicketAsync(
        int ticketId,
        int customerId,
        UpdateTicketDto dto,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteCustomerTicketAsync(
        int ticketId,
        int customerId,
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

    Task<IReadOnlyList<TicketListDto>> GetByAssignedAgentIdAsync(int agentId, CancellationToken cancellationToken = default);
    
    Task<TicketDetailDto?> GetAssignedAgentTicketByIdAsync(
        int ticketId,
        int agentId,
        CancellationToken cancellationToken = default
    );


    Task<TicketAssignmentServiceResult> AssignAgentAsync(
        int ticketId,
        int agentId,
        CancellationToken cancellationToken = default
    );

    Task<TicketStatusUpdateResultDto?> UpdateStatusAsync(
        int ticketId,
        string newStatus,
        int changedByUserId,
        CancellationToken cancellationToken = default
    );

    Task<bool> CanUserManageTicketStatusAsync(
        int ticketId,
        int userId,
        string role,
        CancellationToken cancellationToken = default
    );
}