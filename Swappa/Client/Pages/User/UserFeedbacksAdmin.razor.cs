﻿using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.User
{
    public partial class UserFeedbacksAdmin
    {
        private string count = string.Empty;
        private FeedbackRating averageRating = FeedbackRating.None;
        public PageAndDateDto Query { get; set; } = new();
        public ResponseModel<PaginatedListDto<UserFeedbackDto>>? Data { get; set; }
        public ResponseModel<FeedbackDashboardDto>? FeedbackDashboard { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var dashboard = await UserService.FeedbackDashboardData();
            if(dashboard != null && dashboard.IsSuccessful)
            {
                count = StringifyCount(dashboard.Data.TotalFeedbacks);
                averageRating = dashboard.Data.AverageRating;
            }
            await GetFeedbacks();
            
            await base.OnInitializedAsync();
        }

        private async Task GetFeedbacks()
        {
            var feedbacks = await UserService.GetUsersFeedbacks(Query);
            Data = feedbacks;
        }

        private string StringifyCount(long count)
        {
            const double oneThousand = 1000, 
                oneMillion = 1000000, 
                oneBillion = 1000000000, 
                oneTrillion = 1000000000000;

            double oneThousandth = count / oneThousand, 
                oneMillionth = count / oneMillion, 
                oneBillionth = count / oneBillion, 
                oneTrillionth = count / oneTrillion;

            if(oneThousandth >= 1 && oneThousandth < oneThousand)
            {
                return string.Format("{0:N1}K+", oneThousandth);
            }
            else if(oneMillionth >= 1 && oneMillionth < oneThousand)
            {
                return string.Format("{0:N1}M+", oneMillionth);
            }
            else if(oneBillionth >= 1 && oneBillionth < oneThousand)
            {
                return string.Format("{0:N1}B+", oneBillionth);
            }
            else if (oneTrillionth >= 1 && oneTrillionth < oneThousand)
            {
                return string.Format("{0:N1}T+", oneBillionth);
            }
            else
            {
                return string.Format("{0}", count);
            }
        }
    }
}