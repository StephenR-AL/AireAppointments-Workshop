namespace AireAppointments.Api.Models;

public class Appointment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime AppointmentDateTime { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
