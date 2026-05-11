using FluentAssertions;
using NUnit.Framework;
using AireAppointments.Api.Data;
using AireAppointments.Api.DTOs;
using AireAppointments.Api.Models;
using AireAppointments.Api.Services;


namespace AireAppointments.Api.Tests;

[TestFixture]
public class AuthServiceTests
{
    private AppDbContext _context = null!;
    private AuthService _authService = null!;

    [SetUp]
    public void SetUp()
    {
        _context = TestHelper.CreateInMemoryContext();
        var protector = TestHelper.CreateDataProtectionProvider();
        _authService = new AuthService(_context, protector);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void ValidateCredentials_WithValidCredentials_ReturnsToken()
    {
        _context.Admins.Add(new Admin
        {
            Email = "admin@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        });
        _context.SaveChanges();

        var dto = new LoginDto { Email = "admin@test.com", Password = "password123" };

        var token = _authService.ValidateCredentials(dto);

        token.Should().NotBeNull();
    }

    [Test]
    public void ValidateCredentials_WithInvalidEmail_ReturnsNull()
    {
        var dto = new LoginDto { Email = "nonexistent@test.com", Password = "password123" };

        var token = _authService.ValidateCredentials(dto);

        token.Should().BeNull();
    }

    [Test]
    public void ValidateCredentials_WithInvalidPassword_ReturnsNull()
    {
        _context.Admins.Add(new Admin
        {
            Email = "admin@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        });
        _context.SaveChanges();

        var dto = new LoginDto { Email = "admin@test.com", Password = "wrongpassword" };

        var token = _authService.ValidateCredentials(dto);

        token.Should().BeNull();
    }

    [Test]
    public void GetAdminFromToken_WithValidToken_ReturnsAdmin()
    {
        _context.Admins.Add(new Admin
        {
            Id = 1,
            Email = "admin@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        });
        _context.SaveChanges();

        var dto = new LoginDto { Email = "admin@test.com", Password = "password123" };
        var token = _authService.ValidateCredentials(dto);

        var admin = _authService.GetAdminFromToken(token!);

        admin.Should().NotBeNull();
        admin!.Email.Should().Be("admin@test.com");
    }

    [Test]
    public void GetAdminFromToken_WithInvalidToken_ReturnsNull()
    {
        var admin = _authService.GetAdminFromToken("invalid-token");

        admin.Should().BeNull();
    }

    [Test]
    public void GetAdminFromToken_WithDeletedAdmin_ReturnsNull()
    {
        _context.Admins.Add(new Admin
        {
            Id = 1,
            Email = "admin@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        });
        _context.SaveChanges();

        var dto = new LoginDto { Email = "admin@test.com", Password = "password123" };
        var token = _authService.ValidateCredentials(dto);

        _context.Admins.RemoveRange(_context.Admins);
        _context.SaveChanges();

        var admin = _authService.GetAdminFromToken(token!);

        admin.Should().BeNull();
    }
}
