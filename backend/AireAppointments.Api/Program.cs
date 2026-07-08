using AireAppointments.Api.Data;
using AireAppointments.Api.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDataProtection();
builder.Services.AddSingleton<UtcDateTimeInterceptor>();
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.AddInterceptors(sp.GetRequiredService<UtcDateTimeInterceptor>());
});
builder.Services.AddScoped<AireAppointments.Api.Services.AuthService>();
builder.Services.AddScoped<AireAppointments.Api.Services.IAppointmentService, AireAppointments.Api.Services.AppointmentService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var retries = 10;
    while (true)
    {
        try
        {
            await db.Database.EnsureCreatedAsync();
            DbSeeder.Seed(db);
            break;
        }
        catch (Exception) when (retries > 0)
        {
            retries--;
            await Task.Delay(3000);
        }
    }
}

app.UseCors("AllowFrontend");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuth();
app.MapControllers();

app.Run();
