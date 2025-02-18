﻿using BlazorBootstrap;
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
                var bgColors = SharedService.GetRandomBackgroundColors(data.Count, backgroundColors);
                var dataSets = GetDatasetsNoLabel(data.Values.ToList(), bgColors);

                chartData = new ChartData { Labels = data.Keys.ToList(), Datasets = dataSets };

                doughnutChartOptions = new();
                doughnutChartOptions.Responsive = true;
                doughnutChartOptions.Plugins.Title!.Text = $"Top Five Vehicles - {DateTime.Now.Year}";
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

        private List<IChartDataset> GetDatasetsNoLabel(List<double?> data, List<string> bgColor)
        {
            return new List<IChartDataset>
            {
                GetDataSet(data, bgColor)
            };
        }

        private DoughnutChartDataset GetDataSet(List<double?> data, List<string> bgColor)
        {
            var result = new DoughnutChartDataset { Label = $"", Data = data, BackgroundColor = bgColor };
            return result;
        }
    }
}
