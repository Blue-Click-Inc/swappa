using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Entities.Enums;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Modals.Role
{
    public partial class RoleManagerModal
    {
        private string pageTitle = "Role Manager";
        private bool isLoading = false;
        private bool isError = false;
        private bool isBusy = false;
        private string message = string.Empty;

        [CascadingParameter]
        public BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public Guid Id { get; set; }
        public List<SystemRole> UserRoles { get; set; } = new();
        public SystemRole Role { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetUserByIdAsync(Id);
            await base.OnInitializedAsync();
        }

        private async Task GetUserByIdAsync(Guid id)
        {
            isLoading = true;
            var userResult = await UserService.GetUserByIdAsync(id);
            if (userResult != null && userResult.IsSuccessful)
            {
                UserRoles = userResult.Data.UserRoles;
                pageTitle = $"Manage {userResult.Data.Name} Roles";
            }
            else
            {
                isError = true;
                message = userResult?.Message ?? "An error occurred. Could not get user details";
            }

            isLoading = false;
        }

        private async Task AssignAsync()
        {
            if(Id.IsNotEmpty())
            {
                isBusy = true;
                var response = await AccountService.AssignToRole(Id, Role);
                if(response != null && response.IsSuccessful)
                {
                    Toast.ShowSuccess(response?.Data ?? string.Empty);
                    await GetUserByIdAsync(Id);
                }
                else
                {
                    Toast.ShowError(response?.Message ?? string.Empty);
                }
            }
            else
            {
                Toast.ShowError("Invalid request parameters. Please try again.");
            }

            isBusy = false;
        }

        private async Task RemoveAsync(SystemRole role)
        {
            if (Id.IsNotEmpty())
            {
                isBusy = true;
                var response = await AccountService.RemoveFromRole(Id, role);
                if (response != null && response.IsSuccessful)
                {
                    Toast.ShowSuccess(response?.Data ?? string.Empty);
                    await GetUserByIdAsync(Id);
                }
                else
                {
                    Toast.ShowError(response?.Message ?? string.Empty);
                }
            }
            else
            {
                Toast.ShowError("Invalid request parameters. Please try again.");
            }

            isBusy = false;
        }

        private void OnRoleSelected(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                var role = e.Value.ToString() ?? string.Empty;
                if (role.TryParseValue(out SystemRole roleEnum))
                {
                    Role = roleEnum;
                }
                else
                {
                    Toast.ShowWarning($"Invalid role: {role} selected. Please select a valid role.");
                }
            }
            this.StateHasChanged();
        }
    }
}
