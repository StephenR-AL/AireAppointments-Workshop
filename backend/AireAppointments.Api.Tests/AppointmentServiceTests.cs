using AireAppointments.Api.Data;
using AireAppointments.Api.DTOs;
using AireAppointments.Api.Models;
using AireAppointments.Api.Services;
using FluentAssertions;
using NUnit.Framework;

namespace AireAppointments.Api.Tests;

[TestFixture]
public class AppointmentServiceTests
{
    private AppDbContext _context = null!;
    private AppointmentService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _context = TestHelper.CreateInMemoryContext();
        _service = new AppointmentService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsAppointmentsOrderedByDate()
    {
        _context.Appointments.AddRange(
            new Appointment { Name = "Later", AppointmentDateTime = new DateTime(2026, 6, 1), Description = "d", ContactNumber = "1", EmailAddress = "a@a.com" },
            new Appointment { Name = "Earlier", AppointmentDateTime = new DateTime(2026, 4, 1), Description = "d", ContactNumber = "1", EmailAddress = "a@a.com" }
        );
        await _context.SaveChangesAsync();

        var results = await _service.GetAllAsync();

        results.Should().HaveCount(2);
        results[0].Name.Should().Be("Earlier");
        results[1].Name.Should().Be("Later");
    }

    [Test]
    public async Task GetByIdAsync_WithValidId_ReturnsAppointment()
    {
        var appointment = new Appointment
        {
            Name = "Test",
            AppointmentDateTime = DateTime.UtcNow,
            Description = "desc",
            ContactNumber = "123",
            EmailAddress = "test@test.com"
        };
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var result = await _service.GetByIdAsync(appointment.Id);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Test");
    }

    [Test]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        var result = await _service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Test]
    public async Task CreateAsync_AddsAppointmentToDatabase()
    {
        var dto = new CreateAppointmentDto
        {
            Name = "New Patient",
            AppointmentDateTime = new DateTime(2026, 5, 1, 10, 0, 0),
            Description = "Checkup",
            ContactNumber = "07700900000",
            EmailAddress = "patient@test.com"
        };

        var result = await _service.CreateAsync(dto);

        result.Name.Should().Be("New Patient");
        result.Status.Should().Be(AppointmentStatus.Pending);
        _context.Appointments.Should().HaveCount(1);
    }

    [Test]
    public async Task UpdateAsync_WithValidId_UpdatesAndReturnsAppointment()
    {
        var appointment = new Appointment
        {
            Name = "Old Name",
            AppointmentDateTime = DateTime.UtcNow,
            Description = "desc",
            ContactNumber = "123",
            EmailAddress = "old@test.com"
        };
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var dto = new UpdateAppointmentDto
        {
            Name = "Updated Name",
            AppointmentDateTime = new DateTime(2026, 7, 1),
            Description = "Updated desc",
            ContactNumber = "456",
            EmailAddress = "updated@test.com"
        };

        var result = await _service.UpdateAsync(appointment.Id, dto);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated Name");
        result.EmailAddress.Should().Be("updated@test.com");
    }

    [Test]
    public async Task UpdateAsync_WithInvalidId_ReturnsNull()
    {
        var dto = new UpdateAppointmentDto
        {
            Name = "Name",
            AppointmentDateTime = DateTime.UtcNow,
            Description = "desc",
            ContactNumber = "123",
            EmailAddress = "test@test.com"
        };

        var result = await _service.UpdateAsync(999, dto);

        result.Should().BeNull();
    }

    [Test]
    public async Task DeleteAsync_WithValidId_RemovesAndReturnsTrue()
    {
        var appointment = new Appointment
        {
            Name = "To Delete",
            AppointmentDateTime = DateTime.UtcNow,
            Description = "desc",
            ContactNumber = "123",
            EmailAddress = "del@test.com"
        };
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var result = await _service.DeleteAsync(appointment.Id);

        result.Should().BeTrue();
        _context.Appointments.Should().BeEmpty();
    }

    [Test]
    public async Task DeleteAsync_WithInvalidId_ReturnsFalse()
    {
        var result = await _service.DeleteAsync(999);

        result.Should().BeFalse();
    }

    [Test]
    public async Task ApproveAsync_WithValidId_SetsStatusToApproved()
    {
        var appointment = new Appointment
        {
            Name = "To Approve",
            AppointmentDateTime = DateTime.UtcNow,
            Description = "desc",
            ContactNumber = "123",
            EmailAddress = "ap@test.com",
            Status = AppointmentStatus.Pending
        };
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var result = await _service.ApproveAsync(appointment.Id);

        result.Should().NotBeNull();
        result!.Status.Should().Be(AppointmentStatus.Approved);
    }

    [Test]
    public async Task ApproveAsync_WithInvalidId_ReturnsNull()
    {
        var result = await _service.ApproveAsync(999);

        result.Should().BeNull();
    }
}
