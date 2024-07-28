namespace Swappa.Shared.Extensions
{
    public static class DateExtensions
    {
        private static readonly DateTime MinDate = new DateTime(1700, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 0);

        public static bool IsMaxOrLess(this DateTime target) =>
            target == DateTime.MaxValue || target.AddDays(90) > DateTime.Now;

        public static bool IsLaterThan(this DateTime target, DateTime date) =>
            target.Ticks > date.Ticks;

        public static bool IsValid(this DateTime target)
        {
            return (target.Date >= MinDate.Date) && (target.Date < MaxDate.Date);
        }

        public static bool IsBetween(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            if (dateTime.IsValid())
            {
                return dateTime.Ticks >= startDate.Ticks && dateTime.Ticks <= endDate.Ticks;
            }
            return false;
        }

        /// <summary>
        /// Formats date objects in this format: January 1, 2000. 12:00 AM
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string CommaSeparatedDateTime(this DateTime target)
        {
            if(target.IsValid() && !target.Date.Equals(MaxDate.Date))
            {
                return $"{target:M}, {target:yyyy}. {target:t}";
            }

            return "N/A";
        }

        /// <summary>
        /// Returns date in MMM d, yyyy format: Jun 7, 2000.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToDateStringFormat(this DateTime target)
        {
            var date = target <= MinDate ? 
                DateTime.UtcNow.AddYears(-1) : 
                target >= MaxDate ? 
                DateTime.UtcNow : 
                target;

            return $"{date:MMM} {date:dd}, {date:yyyy}";
        }

        public static DateTime ToEndOfDay(this DateTime target) =>
            new DateTime(target.Year, target.Month, target.Day, 23, 59, 59, 0);

        public static DateTime MinimumDate() =>
            MinDate;

        public static DateTime MaximumDate() =>
            MaxDate;
    }
}
