using System.ComponentModel.DataAnnotations;

namespace AireAppointments.Api.DTOs;

public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime dateTime && dateTime <= DateTime.UtcNow)
        {
            return new ValidationResult(ErrorMessage ?? "Appointment date must be in the future");
        }

        return ValidationResult.Success;
    }
}
