using System.ComponentModel;

namespace BigBallz.Core.Helper
{
    public enum NumberNegativePattern
    {
        [Description("(n)")]
        parenthesis = 0,

        [Description("-n")]
        negative_number = 1,

        [Description("- n")]
        negative_space_number = 2,

        [Description("n-")]
        number_negative = 3,

        [Description("n -")]
        number_space_negative = 4
    }
}
