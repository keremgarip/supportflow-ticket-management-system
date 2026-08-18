using SupportFlow.Api.Models;

namespace SupportFlow.Api.Helpers;

public static class TicketStatusTransitions
{
    private static readonly IReadOnlyDictionary<string, IReadOnlySet<string>>
        AllowedTransitions =
            new Dictionary<string, IReadOnlySet<string>>
            {
                [TicketStatuses.Open] =
                    new HashSet<string>
                    {
                        TicketStatuses.InProgress
                    },
                [TicketStatuses.InProgress] =
                    new HashSet<string>
                    {
                        TicketStatuses.WaitingForCustomer,
                        TicketStatuses.Resolved,
                    },
                [TicketStatuses.WaitingForCustomer] =
                    new HashSet<string>
                    {
                        TicketStatuses.InProgress,
                        TicketStatuses.Resolved
                    },
                [TicketStatuses.Resolved] =
                    new HashSet<string>
                    {
                        TicketStatuses.InProgress,
                        TicketStatuses.Closed
                    },
                [TicketStatuses.Closed] =
                    new HashSet<string>()
            };
    public static bool CanTransition(string currentStatus, string newStatus)
    {
        return AllowedTransitions.TryGetValue(
                    currentStatus,
                    out var allowedStatuses)
               && allowedStatuses.Contains(newStatus);
    }
}