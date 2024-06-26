using Hangfire;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Swappa.Data.Contracts
{
    [Queue("recurring")]
    public interface IRecurringJobService
    {
        [RecurringJob("0 */5 * * *")]
        Task CacheLocationData(PerformContext context);
    }
}
