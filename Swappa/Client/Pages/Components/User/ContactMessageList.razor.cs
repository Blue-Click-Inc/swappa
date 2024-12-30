using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Client.Pages.Modals;
using Swappa.Client.Pages.Modals.User;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Components.User
{
    public partial class ContactMessageList
    {
        [Parameter]
        public PaginatedListDto<ContactMessageToReturnDto> Data { get; set; } = new();
        [Parameter]
        public EventCallback ReloadList { get; set; }
        [Parameter]
        public PageDto Query { get; set; } = new();
        [Parameter]
        public bool IsLoading { get; set; }
        [Parameter]
        public bool HasError { get; set; }
        public HashSet<Guid> Selected { get; set; } = new();

        private async Task ShowMessageDetails(Guid id)
        {
            var param = new ModalParameters
            {
                { "Id", id }
            };

            var modal = Modal.Show<ContactMessageModal>("", param);
            await modal.Result;
            await ReloadList.InvokeAsync();
        }

        private async Task ConfirmAndDelete(Guid id)
        {
            var parameters = SharedService.GetDialogParameters($"Confirm Delete",
                    $"You are about to delete this message. This action is irreversible. Do you wish to proceed?");

            var dialog = Modal.Show<ConfirmationDialog>(parameters);
            var result = await dialog.Result;
            if (result.Confirmed)
            {
                var response = await ContactMessageService.DeprecateAsync(id);
                if(response.IsNotNull() && response.IsSuccessful)
                {
                    Toast.ShowSuccess(response?.Data ?? "Operation successful.");
                    await ReloadList.InvokeAsync();
                }
                else
                {
                    Toast.ShowError(response?.Message ?? "Operation not successful. Please try again later");
                }
            }
        }

        private async Task ConfirmAndDeleteAll()
        {
            if (Selected.IsNotNullOrEmpty())
            {
                var count = Selected.Count;
                var message = count > 1 ? "messages" : "message";
                var parameters = SharedService.GetDialogParameters($"Delete {count} {message}",
                        $"You are about to permanently delete {count} selected {message}. This action is irreversible. Do you wish to proceed?");

                var dialog = Modal.Show<ConfirmationDialog>(parameters);
                var result = await dialog.Result;
                if (result.Confirmed)
                {
                    var response = await ContactMessageService.DeprecateManyAsync(Selected.ToList());
                    if (response.IsNotNull() && response.IsSuccessful)
                    {
                        Toast.ShowSuccess(response?.Data ?? "Operation successful.");
                        await ReloadList.InvokeAsync();
                    }
                    else
                    {
                        Toast.ShowError(response?.Message ?? "Operation not successful. Please try again later");
                    }
                }
            }
        }

        private async Task MarkSelectedAsRead()
        {
            if (Selected.IsNotNullOrEmpty())
            {
                await ReloadList.InvokeAsync();
            }
        }

        private async Task OnPageChangedAsync(int newPageNumber)
        {
            Query.PageNumber = newPageNumber;
            await ReloadList.InvokeAsync();
        }

        private string GetMessage(string str)
        {
            return SharedService.GetSubstring(str);
        }

        private string GetFontWeight(bool isRead) =>
            isRead ? "fw-normal" : "fw-bold";

        private string GetReadIndicator(bool isRead) =>
            isRead ? "text-secondary" : "text-primary";

        public bool AllChecked()
        {
            return Selected.Count == Data.Data.Count();
        }

        public void CheckboxClicked(object? aChecked)
        {
            if ((bool?)aChecked ?? false)
            {
                var ids = Data.Data.Select(x => x.Id).ToList();
                Selected = ids.ToHashSet();
            }
            else
            {
                Selected.RemoveWhere(i => true);
            }
            StateHasChanged();
        }

        public void CheckboxClicked(Guid selectedId, object? aChecked)
        {
            if ((bool?)aChecked ?? false)
            {
                if (!Selected.Contains(selectedId))
                {
                    Selected.Add(selectedId);
                }
            }
            else
            {
                if (Selected.Contains(selectedId))
                {
                    Selected.Remove(selectedId);
                }
            }
            StateHasChanged();
        }
    }
}
