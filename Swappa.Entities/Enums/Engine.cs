using System.ComponentModel;

namespace Swappa.Entities.Enums
{
    public enum Engine : byte
    {
        [Description("None Specified")]
        None,
        [Description("4 Cyl.")]
        FourCylinders,
        [Description("6 Cyl.")]
        SixCylinders = 1 << 1,
        [Description("8 Cyl.")]
        EightCylinders = 1 << 2,
        [Description("12 Cyl.")]
        TwelveCylinders = 1 << 3,
        [Description("16 Cyl.")]
        SixteenCylinders = 1 << 4,
        [Description("20 Cyl.")]
        TwentyCylinders = 1 << 5,
        [Description("24 Cyl.")]
        TwentyFourCylinders = 1 << 6,
        [Description("Others")]
        Others = 1 << 7
    }
}
