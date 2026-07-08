using AireAppointments.Api.Data;
using AireAppointments.Api.DTOs;
using AireAppointments.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AireAppointments.Api.Services;

public class AppointmentService(AppDbContext db) : IAppointmentService
{
    public async Task<List<Appointment>> GetAllAsync()
    {
        return await db.Appointments
            .OrderBy(a => a.AppointmentDateTime)
            .ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await db.Appointments.FindAsync(id);
    }

    public async Task<Appointment> CreateAsync(CreateAppointmentDto dto)
    {
        var appointment = new Appointment
        {
            Name = dto.Name,
            AppointmentDateTime = DateTime.SpecifyKind(dto.AppointmentDateTime, DateTimeKind.Utc),
            Description = dto.Description,
            ContactNumber = dto.ContactNumber,
            EmailAddress = dto.EmailAddress
        };

        db.Appointments.Add(appointment);
        await db.SaveChangesAsync();

        return appointment;
    }

    public async Task<Appointment?> UpdateAsync(int id, UpdateAppointmentDto dto)
    {
        var appointment = await db.Appointments.FindAsync(id);
        if (appointment == null)
            return null;

        appointment.Name = dto.Name;
        appointment.AppointmentDateTime = DateTime.SpecifyKind(dto.AppointmentDateTime, DateTimeKind.Utc);
        appointment.Description = dto.Description;
        appointment.ContactNumber = dto.ContactNumber;
        appointment.EmailAddress = dto.EmailAddress;

        await db.SaveChangesAsync();

        return appointment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var appointment = await db.Appointments.FindAsync(id);
        if (appointment == null)
            return false;

        db.Appointments.Remove(appointment);
        await db.SaveChangesAsync();

        return true;
    }

    public async Task<Appointment?> ApproveAsync(int id)
    {
        var appointment = await db.Appointments.FindAsync(id);
        if (appointment == null)
            return null;

        appointment.Status = AppointmentStatus.Approved;
        await db.SaveChangesAsync();

        return appointment;
    }
}
