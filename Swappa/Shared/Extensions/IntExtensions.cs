namespace Swappa.Shared.Extensions
{
    public static class IntExtensions
    {
        private static readonly decimal Thousand = 1000m;
        private static readonly decimal Million = 1000000m;
        private static readonly decimal Billion = 1000000000;

        public static string ToShortNumString(this long value)
        {
            return value switch
            {
                long num when num >= 0 && num <= 99 => num.ToString(),
                long num when num >= 100 && num < Thousand => "99+",
                long num when num >= Thousand && num < Million => $"{Math.Round(value / Thousand, 1)}k",
                long num when num >= Million && num < Billion => $"{Math.Round(value / Million, 1)}M",
                long num when num >= Billion => $"{Math.Round(value / Billion, 1)}B",
                _ => value.ToString(),
            };
        }
    }
}
