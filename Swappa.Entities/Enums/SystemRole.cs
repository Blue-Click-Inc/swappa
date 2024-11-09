using System.ComponentModel;

namespace Swappa.Entities.Enums
{
    public enum SystemRole
    {
        [Description("User")]
        User,
        [Description("Merchant")]
        Merchant,
        [Description("Admin")]
        Admin,
        [Description("Super Admin")]
        SuperAdmin
    }
}
