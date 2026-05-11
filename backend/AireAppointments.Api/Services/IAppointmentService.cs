using AireAppointments.Api.DTOs;
using AireAppointments.Api.Models;

namespace AireAppointments.Api.Services;

public interface IAppointmentService
{
    Task<List<Appointment>> GetAllAsync();
    Task<Appointment?> GetByIdAsync(int id);
    Task<Appointment> CreateAsync(CreateAppointmentDto dto);
    Task<Appointment?> UpdateAsync(int id, UpdateAppointmentDto dto);
    Task<bool> DeleteAsync(int id);
    Task<Appointment?> ApproveAsync(int id);
}
