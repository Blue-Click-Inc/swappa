namespace Swappa.Shared.Extensions
{
    public static class ListExtensions
    {
        public static bool IsNotNullOrEmpty<T>(this IList<T> target) =>
            target != null && target.Count > 0;

        public static bool IsNullOrEmpty<T>(this IList<T> target) =>
            target == null || target.Count <= 0;

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> target) =>
            target != null && target.Any();

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> target) =>
            target == null || !target.Any();
    }
}
