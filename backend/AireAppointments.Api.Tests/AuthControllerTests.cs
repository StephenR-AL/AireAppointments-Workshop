using AireAppointments.Api.Controllers;
using AireAppointments.Api.Data;
using AireAppointments.Api.DTOs;
using AireAppointments.Api.Models;
using AireAppointments.Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;


namespace AireAppointments.Api.Tests;

[TestFixture]
public class AuthControllerTests
{
    private AppDbContext _context = null!;
    private AuthService _authService = null!;
    private AuthController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _context = TestHelper.CreateInMemoryContext();
        var protector = TestHelper.CreateDataProtectionProvider();
        _authService = new AuthService(_context, protector);
        _controller = new AuthController(_authService)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void Login_WithValidCredentials_SetsCookieAndReturnsOk()
    {
        _context.Admins.Add(new Admin
        {
            Email = "admin@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        });
        _context.SaveChanges();

        var dto = new LoginDto { Email = "admin@test.com", Password = "password123" };

        var result = _controller.Login(dto) as OkObjectResult;

        result.Should().NotBeNull();
        var setCookieHeader = _controller.Response.Headers["Set-Cookie"].ToString();
        setCookieHeader.Should().Contain("aireappointments_auth=");
    }

    [Test]
    public void Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var dto = new LoginDto { Email = "wrong@test.com", Password = "wrong" };

        var result = _controller.Login(dto);

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Test]
    public void Logout_ClearsCookieAndReturnsOk()
    {
        _controller.Response.Cookies.Append("aireappointments_auth", "some-token");

        var result = _controller.Logout() as OkObjectResult;

        result.Should().NotBeNull();
        var setCookieHeader = _controller.Response.Headers["Set-Cookie"].ToString();
        setCookieHeader.Should().Contain("aireappointments_auth=");
        setCookieHeader.Should().Contain("expires=");
    }

    [Test]
    public void Me_WithAuthenticatedUser_ReturnsEmail()
    {
        _controller.HttpContext.Items["AdminEmail"] = "admin@test.com";

        var result = _controller.Me() as OkObjectResult;

        result.Should().NotBeNull();
    }

    [Test]
    public void Me_WithoutAuthentication_ReturnsUnauthorized()
    {
        var result = _controller.Me();

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }
}
