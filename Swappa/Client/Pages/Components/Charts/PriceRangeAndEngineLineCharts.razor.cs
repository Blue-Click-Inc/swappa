using BlazorBootstrap;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Components.Charts
{
    public partial class PriceRangeAndEngineLineCharts
    {
        private bool _isLoading = false;
        private bool _hasError = false;
        private LineChart lineChart = new();
        private LineChartOptions lineChartOptions = default!;
        private ChartData chartData = default!;
        private int dataSetCount = 0;

        protected override async Task OnInitializedAsync()
        {
            await GetPriceRangeAndEngineChartData();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetPriceRangeAndEngineChartData();
                await lineChart.InitializeAsync(chartData, lineChartOptions);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task GetPriceRangeAndEngineChartData()
        {
            _isLoading = true;
            var response = await ChartService.GetPriceRangeAndEngineCharts();
            if (response.IsNotNull() && response.IsSuccessful)
            {
                var data = response.Data;
                chartData = new ChartData { Labels = data.Labels, Datasets = GetDatasets(response.Data.LineCharts) };
                lineChartOptions = new() { Responsive = true, Interaction = new Interaction { Mode = InteractionMode.Index } };

                // set ticks color
                lineChartOptions.Scales.X!.Ticks = new ChartAxesTicks { Color = "red" };
                lineChartOptions.Scales.Y!.Ticks = new ChartAxesTicks { Color = ColorUtility.CategoricalTwelveColors[4] };
                lineChartOptions.Plugins.Title!.Text = "Price Range (₦) By Engine Types";
                lineChartOptions.Plugins.Title.Display = true;
                _isLoading = false;
                return;
            }
            else
            {
                chartData = new();
                _hasError = true;
            }

            _isLoading = false;
        }

        private List<IChartDataset> GetDatasets(List<LineChartsDto> data)
        {
            var result = new List<IChartDataset>();
            foreach (var dataSet in data)
            {
                result.Add(GetDataSet(dataSet));
            }
            
            return result;
        }

        private LineChartDataset GetDataSet(LineChartsDto data)
        {
            dataSetCount++;
            var c = ColorUtility.CategoricalTwelveColors[dataSetCount].ToColor();
            return new LineChartDataset
            {
                Label = data.Label,
                Data = data.Data,
                BackgroundColor = c.ToRgbString(),
                BorderColor = c.ToRgbString(),
                BorderWidth = 2,
                HoverBorderWidth = 4,
            };
        }
    }
}
