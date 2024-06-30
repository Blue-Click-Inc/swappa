using System.ComponentModel;

namespace Swappa.Entities.Enums
{
    public enum SystemRole
    {
        [Description("Regular User")]
        User,
        [Description("Merchant")]
        Merchant,
        [Description("Administrator")]
        Admin,
        [Description("Super Administrator")]
        SuperAdmin
    }
}
