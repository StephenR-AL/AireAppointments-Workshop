using AireAppointments.Api.Models;

namespace AireAppointments.Api.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Admins.Any())
        {
            context.Admins.Add(new Admin
            {
                Email = "admin@aireappointments.co.uk",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
            });
            context.SaveChanges();
        }
    }
}
