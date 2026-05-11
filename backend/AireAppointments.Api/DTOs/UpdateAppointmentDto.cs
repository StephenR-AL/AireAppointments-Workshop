using System.ComponentModel.DataAnnotations;

namespace AireAppointments.Api.DTOs;

public class UpdateAppointmentDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [FutureDate]
    public DateTime AppointmentDateTime { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d+$", ErrorMessage = "Contact number must contain only digits")]
    public string ContactNumber { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;
}
