using AireAppointments.Api.Services;

namespace AireAppointments.Api.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string CookieName = "aireappointments_auth";

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AuthService authService)
    {
        if (context.Request.Cookies.TryGetValue(CookieName, out var token))
        {
            var admin = authService.GetAdminFromToken(token);
            if (admin != null)
            {
                context.Items["AdminId"] = admin.Id;
                context.Items["AdminEmail"] = admin.Email;
            }
        }

        await _next(context);
    }
}

public static class AuthMiddlewareExtensions
{
    public static IApplicationBuilder UseAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}
