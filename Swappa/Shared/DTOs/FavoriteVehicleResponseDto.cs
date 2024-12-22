namespace Swappa.Shared.DTOs
{
    public record FavoriteVehicleResponseDto
    {
        public long Favorites { get; set; }
        public bool IsFavorite { get; set; }
        public long AllFavorites { get; set; }
    }
}
