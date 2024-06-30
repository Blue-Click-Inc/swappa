using Blazored.Modal;
using Microsoft.AspNetCore.Components;

namespace Swappa.Client.Pages.Modals.Role
{
    public partial class DeleteRoleModal
    {
        private bool isLoading = false;
        private string buttonLabel = "Delete";
        private string pageTitle = "Delete Role";

        [CascadingParameter]
        public BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public Guid Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task DeleteAsync()
        {
            isLoading = true;
            var response = await RoleService.DeleteAsync(Id);
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
