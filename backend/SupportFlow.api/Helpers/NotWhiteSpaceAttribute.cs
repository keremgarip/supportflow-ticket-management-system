using System.ComponentModel.DataAnnotations;

namespace SupportFlow.Api.Helpers;

[AttributeUsage(
    AttributeTargets.Property |
    AttributeTargets.Field |
    AttributeTargets.Parameter)]
public sealed class NotWhiteSpaceAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is not string text ||
            !string.IsNullOrWhiteSpace(text);
    }
}