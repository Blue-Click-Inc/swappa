using Blazored.Modal;
using Swappa.Client.Pages.Modals.Role;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Role
{
    public partial class SystemRoles
    {
        private bool isLoading = false;
        private bool hasError = false;
        private string message = string.Empty;
        public long TotalNumberOfUsers { get; set; }

        public PaginatedListDto<RoleDto>? RoleData { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await GetData();
            TotalNumberOfUsers = RoleData?.Data.Sum(r => r.NumberOfUser) ?? 0;
            await base.OnInitializedAsync();
        }

        private async Task GetData()
        {
            isLoading = RoleData == null;

            var result = await RoleService.GetAsync();
            if (result.IsNotNull() && result.IsSuccessful)
            {
                RoleData = result.Data;
            }
            else
            {
                hasError = true;
                message = result?.Message ?? "An unexpected error occurred. Please try again later. Contact support if issue persists.";
            }

            isLoading = false;
        }

        public async Task DeleteRole(Guid id)
        {
            var parameters = new ModalParameters
            {
                { "Id", id }
            };

            var confirmationModal = Modal.Show<DeleteRoleModal>("", parameters);
            var result = await confirmationModal.Result;
            if (result.Confirmed)
            {
                await GetData();
            }
        }

        public async Task UpdateRole(Guid id)
        {
            var parameters = new ModalParameters
            {
                { "Id", id },
            };

            var confirmationModal = Modal.Show<UpdateRoleModal>("", parameters);
            var result = await confirmationModal.Result;
            if (result.Confirmed)
            {
                await GetData();
            }
        }

        public async Task AddRole()
        {
            var confirmationModal = Modal.Show<AddRoleModal>("");
            var result = await confirmationModal.Result;
            if (result.Confirmed)
            {
                await GetData();
            }
        }
    }
}
