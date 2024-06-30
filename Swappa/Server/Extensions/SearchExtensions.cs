using Mongo.Common;
using Swappa.Entities.Models;
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
    }
}
