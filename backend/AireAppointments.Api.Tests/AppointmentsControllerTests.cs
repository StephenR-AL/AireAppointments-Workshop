using AireAppointments.Api.Controllers;
using AireAppointments.Api.DTOs;
using AireAppointments.Api.Models;
using AireAppointments.Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace AireAppointments.Api.Tests;

[TestFixture]
public class AppointmentsControllerTests
{
    private Mock<IAppointmentService> _serviceMock = null!;
    private AppointmentsController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<IAppointmentService>();
        _controller = new AppointmentsController(_serviceMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsAppointmentsOrderedByDate()
    {
        var appointments = new List<Appointment>
        {
            new() { Name = "Earlier", AppointmentDateTime = new DateTime(2026, 4, 1), Description = "d", ContactNumber = "1", EmailAddress = "a@a.com" },
            new() { Name = "Later", AppointmentDateTime = new DateTime(2026, 6, 1), Description = "d", ContactNumber = "1", EmailAddress = "a@a.com" }
        };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(appointments);

        var result = await _controller.GetAll() as OkObjectResult;

        result.Should().NotBeNull();
        var returned = result!.Value as List<Appointment>;
        returned.Should().HaveCount(2);
        returned![0].Name.Should().Be("Earlier");
        returned[1].Name.Should().Be("Later");
    }

    [Test]
    public async Task Get_WithValidId_ReturnsAppointment()
    {
        var appointment = new Appointment
        {
            Id = 1,
            Name = "Test",
            AppointmentDateTime = DateTime.UtcNow,
            Description = "desc",
            ContactNumber = "123",
            EmailAddress = "test@test.com"
        };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(appointment);

        var result = await _controller.Get(1) as OkObjectResult;

        result.Should().NotBeNull();
        var returned = result!.Value as Appointment;
        returned!.Name.Should().Be("Test");
    }

    [Test]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Appointment?)null);

        var result = await _controller.Get(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Create_WithValidData_ReturnsCreated()
    {
        var dto = new CreateAppointmentDto
        {
            Name = "New Patient",
            AppointmentDateTime = new DateTime(2026, 5, 1, 10, 0, 0),
            Description = "Checkup",
            ContactNumber = "07700900000",
            EmailAddress = "patient@test.com"
        };
        var appointment = new Appointment
        {
            Id = 1,
            Name = dto.Name,
            AppointmentDateTime = dto.AppointmentDateTime,
            Description = dto.Description,
            ContactNumber = dto.ContactNumber,
            EmailAddress = dto.EmailAddress,
            Status = AppointmentStatus.Pending
        };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(appointment);

        var result = await _controller.Create(dto) as CreatedResult;

        result.Should().NotBeNull();
        var returned = result!.Value as Appointment;
        returned!.Name.Should().Be("New Patient");
        returned.Status.Should().Be(AppointmentStatus.Pending);
    }

    [Test]
    public async Task Update_WithValidData_ReturnsUpdatedAppointment()
    {
        var dto = new UpdateAppointmentDto
        {
            Name = "Updated Name",
            AppointmentDateTime = new DateTime(2026, 7, 1),
            Description = "Updated desc",
            ContactNumber = "456",
            EmailAddress = "updated@test.com"
        };
        var appointment = new Appointment
        {
            Id = 1,
            Name = dto.Name,
            AppointmentDateTime = dto.AppointmentDateTime,
            Description = dto.Description,
            ContactNumber = dto.ContactNumber,
            EmailAddress = dto.EmailAddress
        };
        _serviceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(appointment);

        var result = await _controller.Update(1, dto) as OkObjectResult;

        result.Should().NotBeNull();
        var updated = result!.Value as Appointment;
        updated!.Name.Should().Be("Updated Name");
        updated.EmailAddress.Should().Be("updated@test.com");
    }

    [Test]
    public async Task Update_WithInvalidId_ReturnsNotFound()
    {
        var dto = new UpdateAppointmentDto
        {
            Name = "Name",
            AppointmentDateTime = DateTime.UtcNow,
            Description = "desc",
            ContactNumber = "123",
            EmailAddress = "test@test.com"
        };
        _serviceMock.Setup(s => s.UpdateAsync(999, dto)).ReturnsAsync((Appointment?)null);

        var result = await _controller.Update(999, dto);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Delete_WithValidId_ReturnsNoContent()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task Delete_WithInvalidId_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

        var result = await _controller.Delete(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Approve_SetsStatusToApproved()
    {
        var appointment = new Appointment
        {
            Id = 1,
            Name = "To Approve",
            AppointmentDateTime = DateTime.UtcNow,
            Description = "desc",
            ContactNumber = "123",
            EmailAddress = "ap@test.com",
            Status = AppointmentStatus.Approved
        };
        _serviceMock.Setup(s => s.ApproveAsync(1)).ReturnsAsync(appointment);

        var result = await _controller.Approve(1) as OkObjectResult;

        result.Should().NotBeNull();
        var approved = result!.Value as Appointment;
        approved!.Status.Should().Be(AppointmentStatus.Approved);
    }

    [Test]
    public async Task Approve_WithInvalidId_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.ApproveAsync(999)).ReturnsAsync((Appointment?)null);

        var result = await _controller.Approve(999);

        result.Should().BeOfType<NotFoundResult>();
    }
}
