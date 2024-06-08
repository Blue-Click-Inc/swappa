using System.ComponentModel;

namespace Swappa.Entities.Enums
{
    public enum FeedbackRating
    {
        [Description("None Rating Selected")]
        None,
        [Description("Very Bad")]
        VeryBad,
        [Description("Bad")]
        Bad,
        [Description("Average")]
        Average,
        [Description("Good")]
        Good,
        [Description("Excellent")]
        Excellent
    }
}
