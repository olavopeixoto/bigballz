using System.ComponentModel;

namespace BigBallz.Core.Helper
{
    public enum CurrencyNegativePattern
    {
        [Description("($n)")]
        parenthesis = 0,
        
        [Description("-$n")]
        negative_symbol_number = 1,
        
        [Description("$-n")]
        symbol_negative_number = 2,
        
        [Description("$n-")]
        symbol_number_negative = 3,

        [Description("(n$)")]
        parenthesis_number_symbol = 4,

        [Description("-n$")]
        negative_number_symbol = 5,

        [Description("n-$")]
        number_negative_symbol = 6,
        
        [Description("n$-")]
        number_symbol_negative = 7,

        [Description("-n $")]
        negative_number_space_symbol = 8,
        
        [Description("-$ n")]
        negative_symbol_space_number = 9,
        
        [Description("n $-")]
        negative_space_symbol_negative = 10,
        
        [Description("$ n-")]
        symbol_space_number_negative = 11,
        
        [Description("$ -n")]
        symbol_space_negative_number = 12,
        
        [Description("n- $")]
        number_negative_space_symbol = 13,
        
        [Description("($ n)")]
         parenthesis_symbol_space_number = 14,
        
        [Description("(n $)")]
        parenthesis_number_space_symbol = 15
    }
}
