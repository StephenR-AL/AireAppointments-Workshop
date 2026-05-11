using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using NUnit.Framework;
using AireAppointments.Api.DTOs;

namespace AireAppointments.Api.Tests;

[TestFixture]
public class AppointmentDtoValidationTests
{
    private static IList<ValidationResult> Validate(object dto)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(dto);
        Validator.TryValidateObject(dto, context, results, validateAllProperties: true);
        return results;
    }

    [Test]
    public void CreateAppointmentDto_WithPastDate_ReturnsValidationError()
    {
        var dto = new CreateAppointmentDto
        {
            Name = "Test",
            AppointmentDateTime = new DateTime(2020, 1, 1),
            Description = "desc",
            ContactNumber = "07700900000",
            EmailAddress = "test@test.com"
        };

        var results = Validate(dto);

        results.Should().ContainSingle(r => r.ErrorMessage!.Contains("future"));
    }

    [Test]
    public void CreateAppointmentDto_WithLettersInContactNumber_ReturnsValidationError()
    {
        var dto = new CreateAppointmentDto
        {
            Name = "Test",
            AppointmentDateTime = DateTime.UtcNow.AddDays(30),
            Description = "desc",
            ContactNumber = "07700abc000",
            EmailAddress = "test@test.com"
        };

        var results = Validate(dto);

        results.Should().ContainSingle(r => r.MemberNames.Contains("ContactNumber"));
    }

    [Test]
    public void CreateAppointmentDto_WithValidData_ReturnsNoValidationErrors()
    {
        var dto = new CreateAppointmentDto
        {
            Name = "Test",
            AppointmentDateTime = DateTime.UtcNow.AddDays(30),
            Description = "desc",
            ContactNumber = "07700900000",
            EmailAddress = "test@test.com"
        };

        var results = Validate(dto);

        results.Should().BeEmpty();
    }

    [Test]
    public void UpdateAppointmentDto_WithPastDate_ReturnsValidationError()
    {
        var dto = new UpdateAppointmentDto
        {
            Name = "Test",
            AppointmentDateTime = new DateTime(2020, 1, 1),
            Description = "desc",
            ContactNumber = "07700900000",
            EmailAddress = "test@test.com"
        };

        var results = Validate(dto);

        results.Should().ContainSingle(r => r.ErrorMessage!.Contains("future"));
    }

    [Test]
    public void UpdateAppointmentDto_WithLettersInContactNumber_ReturnsValidationError()
    {
        var dto = new UpdateAppointmentDto
        {
            Name = "Test",
            AppointmentDateTime = DateTime.UtcNow.AddDays(30),
            Description = "desc",
            ContactNumber = "07700abc000",
            EmailAddress = "test@test.com"
        };

        var results = Validate(dto);

        results.Should().ContainSingle(r => r.MemberNames.Contains("ContactNumber"));
    }

    [Test]
    public void UpdateAppointmentDto_WithValidData_ReturnsNoValidationErrors()
    {
        var dto = new UpdateAppointmentDto
        {
            Name = "Test",
            AppointmentDateTime = DateTime.UtcNow.AddDays(30),
            Description = "desc",
            ContactNumber = "07700900000",
            EmailAddress = "test@test.com"
        };

        var results = Validate(dto);

        results.Should().BeEmpty();
    }
}
