using Microsoft.AspNetCore.Mvc;
using AireAppointments.Api.DTOs;
using AireAppointments.Api.Middleware;
using AireAppointments.Api.Services;

namespace AireAppointments.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController(IAppointmentService appointmentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await appointmentService.GetAllAsync();
        return Ok(appointments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var appointment = await appointmentService.GetByIdAsync(id);
        if (appointment == null)
            return NotFound();

        return Ok(appointment);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var appointment = await appointmentService.CreateAsync(dto);
        return Created($"/api/appointments/{appointment.Id}", appointment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var appointment = await appointmentService.UpdateAsync(id, dto);
        if (appointment == null)
            return NotFound();

        return Ok(appointment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await appointmentService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var appointment = await appointmentService.ApproveAsync(id);
        if (appointment == null)
            return NotFound();

        return Ok(appointment);
    }
}
