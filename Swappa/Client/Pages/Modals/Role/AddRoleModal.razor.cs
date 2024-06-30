using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Role
{
    public partial class AddRoleModal
    {
        private bool isLoading = false;
        private string buttonLabel = "Save";
        private string pageTitle = "Add Role";

        [CascadingParameter]
        public BlazoredModalInstance Instance { get; set; } = new();
        public RoleForCreateDto Model { get; set; } = new();

        private async Task SaveAsync()
        {
            isLoading = true;
            var response = await RoleService.AddAsync(Model);
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