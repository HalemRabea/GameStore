using System.ComponentModel.DataAnnotations;

public record UpdatedGameDto(
    [Required][StringLength(50)] string Name,
    [Range(1, 50)] int GenreId,
    [Range(0, 100)] decimal Price,
    DateOnly ReleaseDate
);