namespace SupportFlow.Api.Helpers;

public enum TicketAssignmentStatus
{
    Success,
    TicketNotFound,
    AgentNotFoundOrInvalid,
    AlreadyAssignedToAgent,
    TicketClosed
}