namespace SupportFlow.Api.Helpers;

public enum TicketStatusUpdateStatus
{
    Success,
    TicketNotFound,
    InvalidTransition,
    AgentAssignmentRequired
}