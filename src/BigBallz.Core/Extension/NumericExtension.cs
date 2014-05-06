using System;
using BigBallz.Core.Helper;
using System.Globalization;

namespace BigBallz.Core
{
	public static class NumericExtensions
	{
		#region Double
		public static string ToPercent(this double value)
		{
			return ToPercent(value, true);
		}

		public static string ToPercent(this double value, bool showSymbol)
		{
			return (value * 100).ToString("N2") + (showSymbol ? "%" : "");
		}

		public static string ToMoney(this double? value)
		{
			return value.HasValue ? ToMoney(value.Value, true, true) : string.Empty;
		}

		public static string ToMoney(this double? value, bool showSymbol)
		{
			return value.HasValue ? ToMoney(value.Value, showSymbol, true) : string.Empty;
		}

		public static string ToMoney(this double? value, bool showSymbol, bool showParenthesis)
		{
			return value.HasValue ? ToMoney(value.Value, showSymbol, showParenthesis) : string.Empty;
		}

		public static string ToMoney(this double value)
		{
			return ToMoney(value, true, true);
		}

		public static string ToMoney(this double value, bool showSymbol)
		{
			return ToMoney(value, showSymbol, true);
		}

		public static string ToMoney(this double value, bool showSymbol, bool showParenthesis)
		{
			return value.ToString(showSymbol ? "C" : "N2", showParenthesis ? CurrencyFormatParenthesis() : CurrencyFormatNoParenthesis());
		}
		#endregion

		#region Decimal
		public static string ToPercent(this decimal value)
		{
			return ToPercent(value, true);
		}

		public static string ToPercent(this decimal value, bool showSymbol)
		{
			return (value * 100).ToString("N2") + (showSymbol ? "%" : "");
		}

		/// <summary>
		/// Formata o número para representação em moeda com o símbolo da moeda em real R$ e entre parentesis
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToMoney(this decimal value)
		{
			return ToMoney(value, true, true);
		}

		/// <summary>
		/// Formata o número para representação em moeda entre parentesis e com a opção do símbolo da moeda em real R$
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToMoney(this decimal value, bool showSymbol)
		{
			return ToMoney(value, showSymbol, true);
		}

		/// <summary>
		/// Formata o número para representação em moeda com a opção de ser entre parentesis e com a opção do símbolo da moeda em real R$
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToMoney(this decimal value, bool showSymbol, bool showParenthesis)
		{
			return value.ToString(showSymbol ? "C" : "N2", showParenthesis ? CurrencyFormatParenthesis() : CurrencyFormatNoParenthesis());
		}

		public static string ToCota(this decimal value)
		{
			return value.ToString("N4");
		}

		public static string ToCota(this decimal? value)
		{
			return value.HasValue ? ToCota(value.Value) : string.Empty;
		}

		public static string ToMoney(this decimal? value)
		{
			return ToMoney(value, true, true);
		}

		public static string ToMoney(this decimal? value, bool showSymbol)
		{
			return value.HasValue ? ToMoney(value.Value, showSymbol, true) : string.Empty;
		}

		public static string ToMoney(this decimal? value, bool showSymbol, bool showParenthesis)
		{
			return value.HasValue ? ToMoney(value.Value, showSymbol, showParenthesis) : string.Empty;
		}

		public static string ToAccountingNotation(this decimal value)
		{
			string strValue;
			var absValor = Math.Abs(value);
			strValue = absValor.ToMoney(false, false);
			var suffix = value < 0 ? " D" : "C";
			strValue += suffix;
			return strValue;
		}

		public static string ToAccountingNotation(this decimal? value)
		{
			return ToAccountingNotation(value ?? 0M);
		}
		#endregion

		public static IFormatProvider CurrencyFormatParenthesis()
		{
			var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone(); ;
			numberFormat.CurrencyDecimalDigits = 2;
			numberFormat.CurrencyDecimalSeparator = ",";
			numberFormat.CurrencyGroupSeparator = ".";
			numberFormat.CurrencyGroupSizes = new int[] { 3 };
			numberFormat.CurrencyNegativePattern = (int)CurrencyNegativePattern.parenthesis_symbol_space_number;
			numberFormat.CurrencyPositivePattern = (int)CurrencyPositivePattern.symbol_space_number;
			numberFormat.CurrencySymbol = "R$";

			numberFormat.NumberNegativePattern = (int)NumberNegativePattern.parenthesis;
			numberFormat.NumberGroupSizes = new int[] { 3 };
			numberFormat.NumberGroupSeparator = ".";
			numberFormat.NumberDecimalSeparator = ",";
			numberFormat.NumberDecimalDigits = 2;

			return numberFormat;
		}
		public static IFormatProvider CurrencyFormatNoParenthesis()
		{
			var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
			numberFormat.CurrencyDecimalDigits = 2;
			numberFormat.CurrencyDecimalSeparator = ",";
			numberFormat.CurrencyGroupSeparator = ".";
			numberFormat.CurrencyGroupSizes = new int[] { 3 };
			numberFormat.CurrencyNegativePattern = (int)CurrencyNegativePattern.symbol_space_negative_number;
			numberFormat.CurrencyPositivePattern = (int)CurrencyPositivePattern.symbol_space_number;
			numberFormat.CurrencySymbol = "R$";

			numberFormat.NumberNegativePattern = (int)NumberNegativePattern.negative_number;
			numberFormat.NumberGroupSizes = new int[] { 3 };
			numberFormat.NumberGroupSeparator = ".";
			numberFormat.NumberDecimalSeparator = ",";
			numberFormat.NumberDecimalDigits = 2;

			return numberFormat;
		}
	}
}