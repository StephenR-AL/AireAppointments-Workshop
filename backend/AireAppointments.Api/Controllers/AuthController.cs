using AireAppointments.Api.DTOs;
using AireAppointments.Api.Middleware;
using AireAppointments.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AireAppointments.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var token = _authService.ValidateCredentials(dto);
        if (token == null)
            return Unauthorized(new { message = "Invalid credentials" });

        Response.Cookies.Append("aireappointments_auth", token, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = false,
            MaxAge = TimeSpan.FromHours(8)
        });

        return Ok(new { message = "Login successful" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("aireappointments_auth");
        return Ok(new { message = "Logged out" });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var email = HttpContext.Items["AdminEmail"] as string;
        if (email == null)
            return Unauthorized(new { message = "Not authenticated" });
        return Ok(new { email });
    }
}
