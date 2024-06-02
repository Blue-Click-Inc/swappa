using System.ComponentModel;

namespace Swappa.Entities.Enums
{
    public enum Gender
    {
        [Description("Prefer not to say")]
        NotSpecified,
        [Description("Male")]
        Male,
        [Description("Female")]
        Female
    }
}
