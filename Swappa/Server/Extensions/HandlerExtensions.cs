using Swappa.Entities.Models;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Extensions
{
    public static class HandlerExtensions
    {
        public static PagedList<Vehicle> MapLocations(this PagedList<Vehicle> vehicles, List<EntityLocation> locations)
        {
            vehicles.ForEach(v =>
            {
                v.Location = locations.FirstOrDefault(l => l.EntityId == v.Id);
            });

            return vehicles;
        }

        public static PagedList<Vehicle> MapImages(this PagedList<Vehicle> vehicles, List<Image> images)
        {
            vehicles.ForEach(v =>
            {
                v.Images = images.Where(l => l.VehicleId == v.Id).ToList();
            });

            return vehicles;
        }
    }
}
