using Swappa.Entities.Models;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Extensions
{
    public static class HandlerExtensions
    {
        public static PagedList<Vehicle> MapLocations(this PagedList<Vehicle> vehicles, Dictionary<Guid, EntityLocation> locations)
        {
            vehicles.ForEach(v =>
            {
                v.Location = locations.GetValueOrDefault(v.Id);
            });

            return vehicles;
        }

        public static PagedList<Vehicle> MapImages(this PagedList<Vehicle> vehicles, Dictionary<Guid, List<Image>> images)
        {
            vehicles.ForEach(v =>
            {
                v.Images = images.GetValueOrDefault(v.Id) ?? new List<Image>();
            });

            return vehicles;
        }
    }
}
