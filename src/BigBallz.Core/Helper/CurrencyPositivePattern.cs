using System.ComponentModel;

namespace BigBallz.Core.Helper
{
    public enum CurrencyPositivePattern
    {
        ///$n
        [Description("$n")]
        symbol_number = 0,

        ///n$
        [Description("n$")]
        number_symbol = 1,
        
        ///$ n
        [Description("$ n")]
        symbol_space_number = 2,

        ///n $
        [Description("n $")]
        number_space_symbol = 3
    }
}
