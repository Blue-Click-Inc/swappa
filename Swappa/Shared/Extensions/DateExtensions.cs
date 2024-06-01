namespace Swappa.Shared.Extensions
{
    public static class DateExtensions
    {
        public static bool IsMaxOrLess(this DateTime target) =>
            target == DateTime.MaxValue || target.AddDays(90) > DateTime.Now;
    }
}
