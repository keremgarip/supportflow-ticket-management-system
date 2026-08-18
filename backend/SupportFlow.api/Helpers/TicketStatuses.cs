namespace SupportFlow.Api.Helpers;

public static class TicketStatuses
{
    public const string Open = "Open";
    public const string InProgress = "In Progress";
    public const string WaitingForCustomer = "Waiting for Customer";
    public const string Resolved = "Resolved";
    public const string Closed = "Closed";
    public static readonly IReadOnlySet<string> All =
        new HashSet<string>(StringComparer.Ordinal)
        {
            Open,
            InProgress,
            WaitingForCustomer,
            Resolved,
            Closed
        };
}