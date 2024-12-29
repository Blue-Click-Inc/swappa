namespace Swappa.Shared.DTOs
{
    public class PriceRangesAndEngineLineChartsDto
    {
        public List<string> Labels { get; set; } = new();
        public List<LineChartsDto> LineCharts { get; set; } = new();
    }

    public class LineChartsDto
    {
        public string Label { get; set; } = string.Empty;
        public List<double?> Data { get; set; } = new();
    }
}
