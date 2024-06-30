using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Modals.Role
{
    public partial class UpdateRoleModal
    {
        private bool isLoading = false;
        private string buttonLabel = "Save";
        private string pageTitle = "Update Role";
        private bool isDataLocading = true;
        private bool isError = false;

        [CascadingParameter]
        public BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public Guid Id { get; set; }
        public RoleForUpdateDto Model { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GetRoleForUpdateAsync(Id);
            await base.OnInitializedAsync();
        }

        private async Task GetRoleForUpdateAsync(Guid Id)
        {
            if (Id.IsNotEmpty())
            {
                var response = await RoleService.GetAsync(Id);
                if(response != null && response.IsSuccessful)
                {
                    Model = new RoleForUpdateDto
                    {
                        RoleName = response.Data.RoleName
                    };
                }
                else
                {
                    isError = true;
                    Toast.ShowError(response?.Message ?? string.Empty);
                    return;
                }
            }
            else
            {
                Toast.ShowError("Invalid Id. Please try again");
            }

            isDataLocading = false;
        }

        private async Task UpdateAsync()
        {
            isLoading = true;
            var response = await RoleService.UpdateAsync(Id, Model);
            if (response != null && response.IsSuccessful)
            {
                Toast.ShowSuccess(response.Data ?? string.Empty);
                await Instance.CloseAsync();
            }
            else
            {
                Toast.ShowError(response?.Message ?? string.Empty);
            }

            isLoading = false;
        }
    }
}
