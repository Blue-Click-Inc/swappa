using Blazored.Modal;
using Swappa.Client.Extensions;
using Swappa.Client.Pages.Modals.Accounts;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Components.Account
{
    public partial class Accounts
    {
        private string _token = string.Empty;
        private TokenType _tokenType;

        protected override async Task OnInitializedAsync()
        {
            await RunVerification();
            await base.OnInitializedAsync();
        }

        async Task RunVerification()
        {
            _token = NavManager.ExtractQueryStringByKey<string>("Token") ?? string.Empty;
            var tokenTypeString = NavManager.ExtractQueryStringByKey<string>("Type") ?? string.Empty;

            if (!tokenTypeString.IsInEnum<TokenType>() ||
                string.IsNullOrWhiteSpace(_token) || !tokenTypeString.TryParseValue(out _tokenType))
            {
                Toast.ShowError("Invalid verification paramters.");
                NavManager.NavigateTo("/", replace: true);
            }
            else
            {
                switch (_tokenType)
                {
                    case TokenType.AccountConfirmation:
                        await RunAccountVerification(_token);
                        break;
                    case TokenType.PasswordReset:
                        RunForgotPassword(_token);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task RunAccountVerification(string token)
        {
            var dto = new AccountConfirmationDto
            {
                Token = token,
            };
            var response = await AccountService.ConfirmAccountAsync(dto);

            if(response != null && response.IsSuccessful)
            {
                Toast.ShowSuccess(response.Data!);
            }
            else
            {
                var errorMessage = response != null ?
                    response.Message :
                    "" ?? string.Empty;

                Toast.ShowError(errorMessage!);
            }

            NavManager.NavigateTo("/", replace: true);
            return;
        }

        private void RunForgotPassword(string token)
        {
            var parameters = new ModalParameters
            {
                { "Token", token }
            };
            Modal.Show<ForgotPasswordModal>("", parameters);
        }
    }
}
