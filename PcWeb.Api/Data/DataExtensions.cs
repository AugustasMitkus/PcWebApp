using System;

namespace PcWeb.Api.Data;

public static class DataExtensions
{
    public static void SetupDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GroupDbContext>();
        dbContext.Database.EnsureCreated();
    }
}
