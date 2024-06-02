using System.ComponentModel;

namespace Swappa.Entities.Enums
{
    public enum Role
    {
        [Description("Regular User")]
        User,
        [Description("Administrator")]
        Admin,
        [Description("Super Administrator")]
        SuperAdmin
    }
}
