namespace Swappa.Shared.Extensions
{
    public static class SharedExtensions
    {
        public static bool GetOrdefault(this bool? boolean) =>
            boolean ?? false;

        public static bool IsNull(this object? value) =>
            value == null;

        public static bool IsNotNull(this object? value) =>
            value != null;
    }
}
