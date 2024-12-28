using BlazorBootstrap;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Components.Charts
{
    public partial class FeedbackPiechart
    {
        //private bool _isLoading = false;
        //private bool _hasError = false;
        //private PieChart pieChart = new();
        //private PieChartOptions pieChartOptions = default!;
        //private ChartData chartData = default!;
        //private string[]? backgroundColors;

        //protected override async Task OnInitializedAsync()
        //{
        //    await GetFeedbackPiechart();
        //    await base.OnInitializedAsync();
        //}

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    await GetFeedbackPiechart();

        //    if (firstRender)
        //    {
        //        await pieChart.InitializeAsync(chartData, pieChartOptions);
        //    }
        //    await base.OnAfterRenderAsync(firstRender);
        //}

        //private async Task GetFeedbackPiechart()
        //{
        //    _isLoading = true;
        //    var response = await ChartService.GetFeedbackPiechartData();
        //    if (response.IsNotNull() && response.IsSuccessful)
        //    {
        //        var data = response.Data;
        //        _isLoading = false;
        //        backgroundColors = ColorUtility.CategoricalTwelveColors;
        //        var bgColors = SharedService.GetRandomBackgroundColors(data.Count, backgroundColors);
        //        var dataSets = SharedService.GetDatasetsNoLabel(data.Values.ToList(), bgColors);
        //        chartData = new ChartData { Labels = data.Keys.ToList(), Datasets = dataSets };

        //        pieChartOptions = new();
        //        pieChartOptions.Responsive = true;
        //        pieChartOptions.Plugins.Title!.Text = $"Feedbacks - {DateTime.Now.Year}";
        //        pieChartOptions.Plugins.Title.Display = true;
        //        pieChartOptions.Plugins.Legend.Position = "top";
        //        return;
        //    }
        //    else
        //    {
        //        chartData = new();
        //        _hasError = true;
        //    }

        //    _isLoading = false;
        //}
    }
}