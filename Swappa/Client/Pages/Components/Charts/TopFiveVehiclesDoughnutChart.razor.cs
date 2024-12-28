using BlazorBootstrap;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Components.Charts
{
    public partial class TopFiveVehiclesDoughnutChart
    {
        private bool _isLoading = false;
        private bool _hasError = false;
        private DoughnutChart doughnutChart = new();
        private DoughnutChartOptions doughnutChartOptions = default!;
        private ChartData chartData = default!;
        private string[]? backgroundColors;

        protected override async Task OnInitializedAsync()
        {
            await GetTopFiveChartData();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetTopFiveChartData();
                await doughnutChart.InitializeAsync(chartData, doughnutChartOptions);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task GetTopFiveChartData()
        {
            _isLoading = true;
            var response = await ChartService.GetTopFiveVehicleCharts();
            if (response.IsNotNull() && response.IsSuccessful)
            {
                var data = response.Data.TopFiveVehicles;
                _isLoading = false;
                backgroundColors = ColorUtility.CategoricalTwelveColors;
                var dataSets = GetDatasets(data.Values.ToList());
                chartData = new ChartData { Labels = data.Keys.ToList(), Datasets = GetDatasets(data.Values.ToList()) };

                doughnutChartOptions = new();
                doughnutChartOptions.Responsive = true;
                doughnutChartOptions.Plugins.Title!.Text = "Top Five Vehicles";
                doughnutChartOptions.Plugins.Title.Display = true;
                return;
            }
            else
            {
                chartData = new();
                _hasError = true;
            }

            _isLoading = false;
        }

        private DoughnutChartDataset GetDataSet(List<double?> data)
        {
            var result = new DoughnutChartDataset { Label = $"", Data = data, BackgroundColor = GetRandomBackgroundColors(data.Count) };
            result.Datalabels.Anchor = Anchor.Start;
            return result;
        }

        private List<string> GetRandomBackgroundColors(int labelsCount)
        {
            var colors = new List<string>();
            for (var index = 0; index < labelsCount; index++)
            {
                colors.Add(backgroundColors![index]);
            }

            return colors;
        }

        private List<IChartDataset> GetDatasets(List<double?> data)
        {
            return new List<IChartDataset>
            {
                GetDataSet(data)
            };
        }
    }
}
