using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using BigBallz.Core.Helper;

namespace BigBallz.Core
{
    public static class StringExtension
    {
        private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex StripHtmlExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly char[] IllegalUrlCharacters = new[] { ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',', '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(', ')', '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘', '’', '“', '”', '»', '«' };

        [DebuggerStepThrough]
        public static string ToHtml(this string target)
        {
            return target.Replace(Environment.NewLine, @"<br>");
        }

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string target, params object[] args)
        {
            Check.Argument.IsNotEmpty(target, "target");

            return string.Format(CultureInfo.CurrentCulture, target, args);
        }

        [DebuggerStepThrough]
        public static string Hash(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(target);
                byte[] hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }

        [DebuggerStepThrough]
        public static string WrapAt(this string target, int index)
        {
            const int dotCount = 3;

            Check.Argument.IsNotNull(target, "target");
            Check.Argument.IsNotNegativeOrZero(index, "index");

            return (target.Length <= index) ? target : string.Concat(target.Substring(0, index - dotCount), new string('.', dotCount));
        }

        [DebuggerStepThrough]
        public static string StripHtml(this string target)
        {
            return StripHtmlExpression.Replace(target, string.Empty);
        }

        [DebuggerStepThrough]
        public static Guid ToGuid(this string target)
        {
            Guid result = Guid.Empty;

            if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
            {
                string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

                try
                {
                    byte[] base64 = Convert.FromBase64String(encoded);

                    result = new Guid(base64);
                }
                catch (FormatException)
                {
                }
            }

            return result;
        }

        [DebuggerStepThrough]
        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        [DebuggerStepThrough]
        public static string ToLegalUrl(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();

            if (target.IndexOfAny(IllegalUrlCharacters) > -1)
            {
                foreach (char character in IllegalUrlCharacters)
                {
                    target = target.Replace(character.ToString(CultureInfo.CurrentCulture), string.Empty);
                }
            }

            target = target.Replace(" ", "-");

            while (target.Contains("--"))
            {
                target = target.Replace("--", "-");
            }

            return target;
        }

        /**/
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            if (value.Length == 1) return value.ToUpperInvariant();

            var lowered = value.ToLowerInvariant();
            return lowered.Substring(0, 1).ToUpperInvariant() + lowered.Substring(1, lowered.Length - 1);
        }

        public static string PascalCaseToWord(this string pascalText)
        {
            if (string.IsNullOrEmpty(pascalText)) return pascalText;

            var sbText = new StringBuilder(pascalText.Length + 4);

            var chars = pascalText.ToCharArray();

            sbText.Append(chars[0]);

            for (var i = 1; i < chars.Length; i++)
            {
                var c = chars[i];
                var prevc = chars[i - 1];
                var nextc = i < chars.Length - 1 ? chars[i + 1] : 'T';

                if (Char.IsUpper(c) && (!Char.IsUpper(prevc) || !Char.IsUpper(nextc)))
                {
                    sbText.Append(' ');
                }

                sbText.Append(c);
            }

            return sbText.ToString();
        }

        /// <summary>
        /// formata um valor sobre uma mascara
        /// </summary>
        /// <param name="valor">valor a formatar</param>
        /// <param name="mascara">no formato ex.:##/##/#### ou ##.###,##</param>
        /// <returns>valor formatado</returns>
        public static string Formatar(this string valor, string mascara)
        {
            var dado = new StringBuilder();
            // remove caracteres nao numericos
            foreach (var c in valor)
            {
                if (Char.IsNumber(c))
                    dado.Append(c);
            }

            var indMascara = mascara.Length;
            var indCampo = dado.Length;

            for (; indCampo > 0 && indMascara > 0; )
            {
                if (mascara[--indMascara] == '#')
                    indCampo--;
            }

            var saida = new StringBuilder();
            for (; indMascara < mascara.Length; indMascara++)
            {
                saida.Append((mascara[indMascara] == '#') ? dado[indCampo++] : mascara[indMascara]);
            }
            return saida.ToString();
        }

        /// <summary>
        /// Formata o CNPJ para o padrão ##.###.###/####-##
        /// </summary>
        /// <param name="cnpj">CNPJ a ser formatado</param>
        /// <returns>Retorna o CNPJ formatado</returns>
        public static string FormatAsCNPJ(this string cnpj)
        {
            var dado = new StringBuilder();
            // remove caracteres nao numericos
            foreach (var c in cnpj)
            {
                if (Char.IsNumber(c))
                    dado.Append(c);

                if (Char.IsLetter(c))
                {
                    dado = new StringBuilder();
                    break;
                }
            }

            var cnpjPreFormat = dado.ToString().PadLeft(14, '0');

            const string padrao = @"^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})";
            var r = new Regex(padrao);
            var m = r.Match(cnpjPreFormat);

            return string.Format("{0}.{1}.{2}/{3}-{4}", m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value, m.Groups[5].Value);
        }

        /// <summary>
        /// Formata o CPF para o padrão ###.###.###-##
        /// </summary>
        /// <param name="cpf">CPF a ser formatado</param>
        /// <returns>Retorna o CPF formatado</returns>
        public static string FormatAsCPF(this string cpf)
        {
            var dado = new StringBuilder();
            // remove caracteres nao numericos
            foreach (var c in cpf)
            {
                if (Char.IsNumber(c))
                    dado.Append(c);

                if (Char.IsLetter(c))
                {
                    dado = new StringBuilder();
                    break;
                }
            }

            var cpfPreFormat = dado.ToString().PadLeft(11, '0');

            const string padrao = @"^(\d{3})(\d{3})(\d{3})(\d{2})";
            var r = new Regex(padrao);
            var m = r.Match(cpfPreFormat);

            return string.Format("{0}.{1}.{2}-{3}", m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value);
        }

        public static string Abbreviation(this string name)
        {
            return name.Replace("Aposentadoria", "Aposent.").Replace("por ", "").Replace("Benefício Proporcional Diferido", "BPD").Replace("Auxílio", "Aux.").Replace("Auxilio", "Aux.");
        }

        public static string Conditional(this string value, bool condition)
        {
            return condition ? value : string.Empty;
        }
    }
}