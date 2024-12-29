using BlazorBootstrap;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Components.Charts
{
    public partial class FeedbackPiechart
    {
        private bool _isLoading = false;
        private bool _hasError = false;
        private PieChart pieChart = new();
        private PieChartOptions pieChartOptions = default!;
        private ChartData chartData = default!;
        private string[]? backgroundColors;

        protected override async Task OnInitializedAsync()
        {
            await InitializeDataAsync();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeDataAsync();
                await pieChart.InitializeAsync(chartData, pieChartOptions);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task InitializeDataAsync()
        {
            backgroundColors = ColorUtility.CategoricalTwelveColors;
            pieChartOptions = new();
            pieChartOptions.Responsive = true;
            pieChartOptions.Plugins.Title!.Text = $"Users' Feedback - {DateTime.Now.Year}";
            pieChartOptions.Plugins.Title.Display = true;

            _isLoading = true;
            var response = await ChartService.GetFeedbackPiechartData();
            if (response.IsNotNull() && response.IsSuccessful)
            {
                var data = response.Data;
                var dataLabels = data.Keys.ToList();
                chartData = new ChartData { Labels = dataLabels, Datasets = GetDataSets(dataLabels.Count, data.Values.ToList()) };
            }
            else
            {
                chartData = new();
                _hasError = true;
            }

            _isLoading = false;
        }

        private List<IChartDataset> GetDataSets(int numberOfDataLabels, List<double?> dataSets)
        {
            var datasets = new List<IChartDataset>();
            datasets.Add(GetPieChartDataset(numberOfDataLabels, dataSets));
            return datasets;
        }

        private PieChartDataset GetPieChartDataset(int numberOfDataLabels, List<double?> dataSets)
        {
            var bgColor = SharedService.GetRandomBackgroundColors(numberOfDataLabels, backgroundColors);
            return new() { Label = "", Data = dataSets, BackgroundColor = bgColor };
        }
    }
}