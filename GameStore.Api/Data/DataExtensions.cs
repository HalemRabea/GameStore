using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("GameStore");
        builder.Services.AddSqlite<GameStoreContext>(
            connectionString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Id = 1, Name = "Action" },
                        new Genre { Id = 2, Name = "Adventure" },
                        new Genre { Id = 3, Name = "RPG" },
                        new Genre { Id = 4, Name = "Strategy" }
                    );
                    context.SaveChanges();
                }
            })
        );
    }
}
