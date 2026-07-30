using System.ComponentModel.DataAnnotations;

namespace SupportFlow.Api.Helpers;

[AttributeUsage(
    AttributeTargets.Property |
    AttributeTargets.Field |
    AttributeTargets.Parameter)]
public sealed class AllowedValuesAttribute : ValidationAttribute
{
    private readonly HashSet<string> _allowedValues;

    public AllowedValuesAttribute(params string[] allowedValues)
    {
        _allowedValues = new HashSet<string>(
            allowedValues,
            StringComparer.Ordinal);
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        return value is string text &&
               _allowedValues.Contains(text);
    }

    public override string FormatErrorMessage(string name)
    {
        return ErrorMessage ??
               $"{name} must be one of the allowed values: " +
               $"{string.Join(", ", _allowedValues)}.";
    }
}