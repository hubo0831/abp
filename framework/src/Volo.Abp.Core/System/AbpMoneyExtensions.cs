namespace System
{
    /// <summary>
    /// Extension methods for the money.
    /// </summary>
    public static class AbpMoneyExtensions
    {
        /// <summary>把字符串(单位为元)转换为货币(单位为元)</summary>
        public static decimal ToCurrency(this string value)
        {
            return decimal.Parse(value);
        }
        /// <summary>把整型值(单位为分)转换为货币(单位为元)</summary>
        public static decimal ToCentiCurrency(this int value)
        {
            return (decimal)value / 100;
        }
        /// <summary>把货币值(单位为元)转换为整型值货币(单位为分)</summary>
        public static int ToHundredCurrency(this decimal value)
        {
            return (int)Math.Floor(value * 100);
        }
        /// <summary>把货币(单位为元)转换为字符串(单位为元)</summary>
        public static string ToCurrencyString(this decimal value)
        {
            return value.ToString("0.00");
        }
        /// <summary>把整型值(单位为分)转换为货币字符串(单位为元)</summary>
        public static string ToCentiCurrencySring(this int value)
        {
            return value.ToCentiCurrency().ToCurrencyString();
        }
    }
}