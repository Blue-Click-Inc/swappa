namespace Swappa.Data.Contracts
{
    public interface IRepositoryManager
    {
        ITokenRepository Token { get; }
        IUserRepository User { get; }
        IUserFeedbackRepository Feedback { get; }
        ILocationService Location { get; }
        IVehicleRepository Vehicle { get; }
        IImageRepository Image { get; }
        ICommonRepository Common { get; }
        IVehicleViewsRepository VehicleViews { get; }
        IFavoriteVehiclesRepository FavoriteVehicles { get; }
    }
}
