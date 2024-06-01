using System.ComponentModel;

namespace Swappa.Entities.Enums
{
    public enum TokenType
    {
        [Description("Account Confirmation")]
        AccountConfirmation,
        [Description("Account Reactivation")]
        AccountReactivation,
        [Description("Password Reset")]
        PasswordReset
    }
}
