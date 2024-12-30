using Mongo.Common;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Extensions
{
    public static class SearchExtensions
    {
        public static IQueryable<T> FilterByDate<T>(this IQueryable<T> list, DateTime startDate, DateTime endDate) where T : IBaseEntity
        {
            if (!startDate.IsValid() || !endDate.IsValid())
                return list;

            return list.Where(l => l.CreatedAt >= startDate && l.CreatedAt <= endDate.ToEndOfDay());
        }

        public static IQueryable<AppUser> Search(this IQueryable<AppUser> users, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return users;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return users.Where(u => u.Name.ToLower().Contains(lowerCaseTerm)
                        || u.Email.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Vehicle> Search(this IQueryable<Vehicle> vehicles, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return vehicles;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return vehicles.Where(u => u.Model.ToLower().Contains(lowerCaseTerm)
                        || u.Make.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<UserFeedback> Search(this IQueryable<UserFeedback> faqs, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return faqs;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return faqs.Where(uf => uf.UserName.ToLower().Contains(lowerCaseTerm)
                        || uf.UserEmail.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Faq> Search(this IQueryable<Faq> faqs, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return faqs;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return faqs.Where(u => u.Title.ToLower().Contains(lowerCaseTerm)
                        || u.Details.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<ContactMessage> Search(this IQueryable<ContactMessage> faqs, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return faqs;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return faqs.Where(u => u.Name.ToLower().Contains(lowerCaseTerm)
                        || u.EmailAddress.ToLower().Contains(lowerCaseTerm)
                        || u.Message.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Vehicle> Filter(this IQueryable<Vehicle> vehicles, VehicleQueryDto filter)
        {
            if (filter.IsAllDefault())
                return vehicles;

            if((filter.MinYear != default || filter.MaxYear != default) && filter.MaxYear > filter.MinYear)
            {
                vehicles = vehicles.Where(v => v.Year >= filter.MinYear && v.Year <= filter.MaxYear);
            }
            if((filter.MinPrice != default || filter.MaxPrice != default) && filter.MaxPrice > filter.MinPrice)
            {
                vehicles = vehicles.Where(v => v.Price >= filter.MinPrice && v.Price <= filter.MaxPrice);
            }
            if(filter.Engine != Engine.None)
            {
                vehicles = vehicles.Where(v => v.Engine == filter.Engine);
            }
            if (filter.Transmission != Transmission.None)
            {
                vehicles = vehicles.Where(v => v.Transmission == filter.Transmission);
            }
            if (filter.DriveTrain != DriveTrain.None)
            {
                vehicles = vehicles.Where(v => v.DriveTrain == filter.DriveTrain);
            }

            return vehicles;
        }

        private static bool IsAllDefault(this VehicleQueryDto query)
        {
            return query == null || (query?.MinPrice == default && query?.MinYear == default &&
                query?.DriveTrain == DriveTrain.None && query?.Transmission == Transmission.None &&
                query?.Engine == Engine.None);
        }
    }
}
