using Hangfire.Console;
using Hangfire.Server;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Shared.Extensions;

namespace Swappa.Data.Services
{
    public class ToolService : IToolService
    {
        private readonly IRepositoryManager repository;

        public ToolService(IRepositoryManager repository)
        {
            this.repository = repository;
        }

        public async Task VehicleBulkUpload(List<Vehicle> vehicles, List<Entities.Models.Image> images, Guid userId, PerformContext context)
        {
            var locations = new List<EntityLocation>();

            if (userId.IsNotEmpty())
            {
                context.WriteLine($"Getting location for user: {userId}");
                var userLocation = await repository.Location.FindOneAsync(_ => _.EntityId.Equals(userId));
                if(userLocation != null)
                {
                    context.WriteLine($"Initializing locations for {vehicles.Count} vehicle records");
                    vehicles.ForEach(v =>
                    {
                        locations.Add(new EntityLocation
                        {
                            EntityId = v.Id,
                            EntityType = EntityType.Vehicle,
                            City = userLocation.City,
                            Country = userLocation.Country,
                            State = userLocation.State,
                            PostalCode = userLocation.PostalCode,
                            CountryId = userLocation.CountryId,
                            StateId = userLocation.StateId,
                            Coordinate = userLocation.Coordinate
                        });
                    });
                }
            }

            context.WriteLine($"Adding {vehicles.Count} vehicle records");
            await repository.Vehicle.AddAsync(vehicles);
            context.WriteLine($"Successfully added {vehicles.Count} vehicle records. Now adding the respective images.");

            await repository.Image.AddAsync(images);
            context.WriteLine($"Successfully added {images.Count} image records. Now adding vehicle locations.");

            await repository.Location.AddAsync(locations);
            context.WriteLine($"Successfully added {locations.Count} location records.");
        }
    }
}
