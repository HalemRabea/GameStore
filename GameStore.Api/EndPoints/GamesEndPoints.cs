using GameStore.Api.Data;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.EndPoints;

public static class GamesEndPoints
{
    const string GetGameEndpointName = "GetName";
 
    public static void MapGamesEndpoints(this IEndpointRouteBuilder app)
    {

        var group = app.MapGroup("/games");
        // Get Games
        group.MapGet("/", async(GameStoreContext dbContext) =>
        await dbContext.Games
        .Include(game => game.Genre)
        .Select(game => game.ToGameGenreDetailsDto()).AsNoTracking().ToListAsync()
        );

        // Get Genres
        group.MapGet("/genres", async (GameStoreContext dbContext) =>
        await dbContext.Genres.Select(genre => new { genre.Id, genre.Name }).ToListAsync()
        );

        // Get Game/1
        group.MapGet("/{id}", async (int id,GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

        }
            ).WithName(GetGameEndpointName);

        // Post Game
        group.MapPost("/", async (CreateGameDto newGame,GameStoreContext dbContext) =>
        {
            Game game = new Game
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = game.ToGameDetailsDto();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        // Put game/1
        group.MapPut("/{id}", async (int id, UpdatedGameDto updatedGame,GameStoreContext dbContext) =>
        {
           
           var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = existingGame.Id }, existingGame.ToGameDetailsDto());
            // return Results.NoContent();
        });

        // Delete game/1
        group.MapDelete("/{id}", async(int id ,GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }
}

