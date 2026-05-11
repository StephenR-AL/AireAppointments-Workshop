using FluentAssertions;
using NUnit.Framework;
using AireAppointments.Api.Data;
using AireAppointments.Api.Models;

namespace AireAppointments.Api.Tests;

[TestFixture]
public class DbSeederTests
{
    private AppDbContext _context = null!;

    [SetUp]
    public void SetUp()
    {
        _context = TestHelper.CreateInMemoryContext();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void Seed_WhenNoAdminsExist_SeedsAdminUser()
    {
        DbSeeder.Seed(_context);

        _context.Admins.Should().HaveCount(1);
        var admin = _context.Admins.First();
        admin.Email.Should().Be("admin@aireappointments.co.uk");
        BCrypt.Net.BCrypt.Verify("admin123", admin.PasswordHash).Should().BeTrue();
    }

    [Test]
    public void Seed_WhenAdminsExist_DoesNotDuplicate()
    {
        _context.Admins.Add(new Admin
        {
            Email = "existing@test.com",
            PasswordHash = "hash"
        });
        _context.SaveChanges();

        DbSeeder.Seed(_context);

        _context.Admins.Should().HaveCount(1);
        _context.Admins.First().Email.Should().Be("existing@test.com");
    }
}
