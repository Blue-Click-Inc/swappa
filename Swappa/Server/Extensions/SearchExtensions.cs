using Mongo.Common;
using Swappa.Shared.Extensions;
using System.Linq.Expressions;

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
    }
}
