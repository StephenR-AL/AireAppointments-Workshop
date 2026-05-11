using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using AireAppointments.Api.Data;

namespace AireAppointments.Api.Tests;

public static class TestHelper
{
    public static AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    public static IDataProtectionProvider CreateDataProtectionProvider()
    {
        return DataProtectionProvider.Create("AireAppointments.Tests");
    }
}
