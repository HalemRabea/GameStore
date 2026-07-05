using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DtoExtensions
{
     public static GameDetailsDto ToGameDetailsDto(this Game game)
    {
        return new GameDetailsDto(
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
        );
    }
     public static GameDto ToGameGenreDetailsDto(this Game game)
    {
        return new GameDto(
            game.Id,
            game.Name,
            game.Genre?.Name??"Unknown",
            game.Price,
            game.ReleaseDate
        );
    }
}
