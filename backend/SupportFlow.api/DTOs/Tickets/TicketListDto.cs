namespace SupportFlow.Api.DTOs.Tickets;

public class TicketListDto
{
    public int Id {get; set;}
    public string Title {get; set;} = string.Empty;
    public string Status {get; set;} = string.Empty;
    public string Priority {get; set;} = string.Empty;
    public int CategoryId {get; set;}
    public string CategoryName {get; set;} = string.Empty;
    public int CustomerId {get; set;}
    public string CustomerName {get; set;} = string.Empty;
    public int? AssignedAgentId {get; set;}
    public string? AssignedAgentName {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}
}