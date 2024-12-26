using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Client.Pages.Modals.Role;
using Swappa.Client.Pages.Modals.User;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components.User
{
    public partial class UserTableList
    {
        [Parameter]
        public PaginatedListDto<LeanUserDetailsDto>? UserData { get; set; }
        [Parameter]
        public EventCallback ShowAddUserModal { get; set; }
        [Parameter]
        public EventCallback GetDashboard { get; set; }
        [Parameter]
        public EventCallback GetPagedUserList { get; set; }
        [Parameter]
        public bool IsLoading { get; set; }

        private async Task OpenUserDetailModal(Guid id)
        {
            var parameters = new ModalParameters
            {
                { "Id", id }
            };

            var confirm = Modal.Show<ViewUserDetailsModal>("", parameters);
            var result = await confirm.Result;
            if (result.Confirmed || result.Cancelled)
            {
                IsLoading = true;
                await GetDashboard.InvokeAsync();
                await GetPagedUserList.InvokeAsync();
                IsLoading = false;
            }
        }

        private async Task OpenRoleManagerModal(Guid id)
        {
            var parameters = new ModalParameters
            {
                { "Id", id }
            };

            var confirm = Modal.Show<RoleManagerModal>("", parameters);
            await confirm.Result;
        }

        private string GetStatusClass(Status status)
        {
            switch (status)
            {
                case Status.Inactive:
                    return "danger";
                case Status.Active:
                    return "success";
                default:
                    return "secondary";
            }
        }
    }
}
