namespace Swappa.Shared.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsEmpty(this Guid target) =>
            target == Guid.Empty;

        public static bool IsNotEmpty(this Guid target) =>
            target != Guid.Empty;

        public static bool IsNullOrEmpty(this Guid? target) =>
            target == null || target ==  Guid.Empty;

        public static Guid ToGuid(this string? target)
        {
            _ = Guid.TryParse(target, out var result);
            return result;
        }
    }
}
