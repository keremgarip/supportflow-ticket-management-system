using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.MicrosoftExtensions;
using SupportFlow.Api.Helpers;
using AllowedValuesAttribute = SupportFlow.Api.Helpers.AllowedValuesAttribute;

namespace SupportFlow.Api.DTOs.Tickets;

public class UpdateTicketStatusDto
{
    [Required]
    [AllowedValues(
        TicketStatuses.Open,
        TicketStatuses.InProgress,
        TicketStatuses.WaitingForCustomer,
        TicketStatuses.Resolved,
        TicketStatuses.Closed,
        ErrorMessage = 
            "Status must be Open, In Progress, Waiting for Customer, Resolved or Closed."
    )]
    public string Status {get; set;} = string.Empty;
}