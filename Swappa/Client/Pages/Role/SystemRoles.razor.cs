using Blazored.Modal;
using Swappa.Client.Pages.Modals.Role;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Role
{
    public partial class SystemRoles
    {
        private bool isLoading = false;
        public long TotalNumberOfUsers { get; set; }

        public PaginatedListDto<RoleDto>? RoleData { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await GetData();
            TotalNumberOfUsers = RoleData.Data.Sum(r => r.NumberOfUser);
            await base.OnInitializedAsync();
        }

        private async Task GetData()
        {
            isLoading = RoleData == null;

            var result = await RoleService.GetAsync();
            if (result != null)
            {
                RoleData = result;
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
