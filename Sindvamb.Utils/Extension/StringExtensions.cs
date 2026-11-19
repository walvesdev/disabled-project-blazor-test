using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using Sindvamb.Utils.Extension;

namespace Sindvamb.Utils.Extension
{
    public static class StringExtension
    {
        public static string RemoveAccents(this string text)
        {
            var sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (var letter in arrayText)
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);

            return sbReturn.ToString();
        }

        public static string OnlyDigits(this string val)
        {
            if (val == null) return val;

            try
            {
                var regexObj = new Regex(@"[^\d]");
                return regexObj.Replace(val, "");
            }
            catch (Exception ex)
            {
                // Syntax error in the regular expression
            }

            return val;
        }

        public static string OnlyNumbersToString(this string val)
        {
            if (val == null) return val;

            try
            {
                return new string(val.Where(char.IsDigit).ToArray());
                ;
            }
            catch (Exception ex)
            {
                // Syntax error in the regular expression
            }

            return val;
        }

        public static string SubstringPerWord(this string val, int startIndex, int endIndex = 0)
        {
            if (endIndex == 0)
            {
                for (var x = startIndex; x > 0; x--)
                    if (val[x] == ' ')
                        return val.Substring(x);
            }
            else
            {
                for (var x = endIndex; x > 0; x--)
                    if (val[x] == ' ')
                        return val.Substring(startIndex, x);
            }

            return val.Substring(startIndex, endIndex == 0 ? val.Length - 1 : endIndex);
        }

        public static IEnumerable<string> SplitBy(this string str, int chunkLength)
        {
            for (var i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length) chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }

        public static string RemoveManySpaces(this string val)
        {
            var options = RegexOptions.None;
            var regex = new Regex("[ ]{2,}", options);
            return regex.Replace(val, " ");
        }

        public static List<int> OnlyNumbers(this string val)
        {
            var result = new List<int>();

            if (string.IsNullOrWhiteSpace(val)) return result;

            val = val.Trim();
            var aux = string.Empty;

            for (var i = 0; i < val.Count(); i++)
                if (char.IsDigit(val[i]))
                {
                    aux += val[i];
                }
                else if (aux.Count() > 0)
                {
                    result.Add(int.Parse(aux));
                    aux = string.Empty;
                }

            if (aux.Count() > 0) result.Add(int.Parse(aux));

            return result;
        }

        public static string ZipString(this string value)
        {
            //Transform string into byte[]  
            var byteArray = new byte[value.Length];
            var indexBA = 0;
            foreach (var item in value.ToCharArray()) byteArray[indexBA++] = (byte)item;

            //Prepare for compress
            var ms = new MemoryStream();
            var sw =
                new GZipStream(ms, CompressionMode.Compress);

            //Compress
            sw.Write(byteArray, 0, byteArray.Length);
            //Close, DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            //Transform byte[] zip data to string
            byteArray = ms.ToArray();
            var sB = new StringBuilder(byteArray.Length);
            foreach (var item in byteArray) sB.Append((char)item);

            ms.Close();
            sw.Dispose();
            ms.Dispose();
            return sB.ToString();
        }

        public static string UnZipString(this string value)
        {
            //Transform string into byte[]
            var byteArray = new byte[value.Length];
            var indexBA = 0;
            foreach (var item in value.ToCharArray()) byteArray[indexBA++] = (byte)item;

            //Prepare for decompress
            var ms = new MemoryStream(byteArray);
            var sr = new GZipStream(ms,
                CompressionMode.Decompress);

            //Reset variable to collect uncompressed result
            byteArray = new byte[byteArray.Length];

            //Decompress
            var rByte = sr.Read(byteArray, 0, byteArray.Length);

            //Transform byte[] unzip data to string
            var sB = new StringBuilder(rByte);
            //Read the number of bytes GZipStream red and do not a for each bytes in
            //resultByteArray;
            for (var i = 0; i < rByte; i++) sB.Append((char)byteArray[i]);

            sr.Close();
            ms.Close();
            sr.Dispose();
            ms.Dispose();
            return sB.ToString();
        }

        public static string GetStringSha256Hash(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            using (var sha = SHA256.Create())
            {
                var textData = Encoding.UTF8.GetBytes(text);
                var hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }


        public static string Replace(this string text, IDictionary<string, string> args)
        {
            var re = new Regex(@"\[(\w+)\]", RegexOptions.Compiled);
            return re.Replace(text, match => args[match.Groups[1].Value]);
        }

        /// <summary>Filtra somente os caracteres decimais na string</summary>
        /// <param name="source">Texto a ser filtrado</param>
        /// <returns></returns>
        public static string GetDigits(this string? source)
        {
            if (source.IsNull()) return string.Empty;

            var stringBuilder = new StringBuilder();
            foreach (var c in source!)
            {
                if (char.IsDigit(c))
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }

        public static string GetCardNumberPart(this string cardNumber)
        {
            if (!string.IsNullOrWhiteSpace(cardNumber) && cardNumber.Length >= 4)
            {
                var temp = cardNumber.GetDigits();
                return temp.Substring(temp.Length - 4);
            }

            return cardNumber;
        }

        public static string TruncateIfNeeded(this string value, int maxlength)
        {
            if (!string.IsNullOrWhiteSpace(value) && value.Length > maxlength) return value.Substring(0, maxlength);

            return value;
        }

        /// <summary>
        ///     Adiciona um caractere ao final de uma determinada string se não terminar com o caractere.
        /// </summary>
        public static string EnsureEndsWith(this string str, char c,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str.EndsWith(c.ToString(), comparisonType)) return str;

            return str + c;
        }

        /// <summary>
        ///     Adiciona um caractere ao início de uma determinada string se não começar com o caractere.
        /// </summary>
        public static string EnsureStartsWith(this string str, char c,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str.StartsWith(c.ToString(), comparisonType)) return str;

            return c + str;
        }

        /// <summary>
        ///     Indica se esta string é nula ou uma string System.String.Empty.
        /// </summary>
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        /// <summary>
        ///     indica se esta string é nula, vazia ou consiste apenas em caracteres de espaço em branco.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        /// <summary>
        ///     Obtém uma substring de uma string do início da string.
        /// </summary>
        public static string Left(this string str, int len)
        {
            if (str.Length < len) throw new Exception("len argument can not be bigger than given string's length!");

            return str.Substring(0, len);
        }

        /// <summary>
        ///     Converter terminações de linha na string para  <see cref="P:System.Environment.NewLine" />.
        /// </summary>
        public static string NormalizeLineEndings(this string str) =>
            str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);

        /// <summary>
        ///     Obtém o índice da enésima ocorrência de um char em uma string.
        /// </summary>
        /// <param name="str">string de origem a ser pesquisada</param>
        /// <param name="c">Char para pesquisar em <paramref name="str" /></param>
        /// <param name="n">Contagem da ocorrência</param>
        public static int NthIndexOf(this string str, char c, int n)
        {
            var count = 0;
            for (var i = 0; i < str.Length; i++)
                if (str[i] == c && ++count == n)
                    return i;

            return -1;
        }

        /// <summary>
        ///     Remova a primeira ocorrência do postfix fornecido do final da string fornecida.
        /// </summary>
        /// <param name="str">a string.</param>
        /// <param name="postFixes">um ou mais postfix.</param>
        /// <returns>String modificada ou a mesma string se não tiver nenhum dos postfixes fornecidos</returns>
        public static string RemovePostFix(this string str, params string[] postFixes) =>
            str.RemovePostFix(StringComparison.Ordinal, postFixes);

        /// <summary>
        ///     Remova a primeira ocorrência do postfix fornecido do final da string fornecida.
        /// </summary>
        /// <param name="str">a string.</param>
        /// <param name="comparisonType">Tipo de comparação de string</param>
        /// <param name="postFixes">um ou mais postfix.</param>
        /// <returns>String modificada ou a mesma string se não tiver nenhum dos postfixes fornecidos</returns>
        public static string RemovePostFix(this string str, StringComparison comparisonType, params string[] postFixes)
        {
            if (str.IsNullOrEmpty()) return str;

            if (postFixes.IsNullOrEmpty()) return str;

            foreach (var postFix in postFixes)
                if (str.EndsWith(postFix, comparisonType))
                    return str.Left(str.Length - postFix.Length);

            return str;
        }

        /// <summary>
        ///     Remove a primeira ocorrência dos prefixos fornecidos do início da string fornecida.
        /// </summary>
        /// <param name="str">A string.</param>
        /// <param name="preFixes">um ou mais prefixo.</param>
        /// <returns>String modificada ou a mesma string se não tiver nenhum dos prefixos fornecidos</returns>
        public static string RemovePreFix(this string str, params string[] preFixes) =>
            str.RemovePreFix(StringComparison.Ordinal, preFixes);

        /// <summary>
        ///     Remove a primeira ocorrência dos prefixos fornecidos do início da string fornecida.
        /// </summary>
        /// <param name="str">A string.</param>
        /// <param name="comparisonType">Tipo de comparação de string</param>
        /// <param name="preFixes">um ou mais prefixo.</param>
        /// <returns>String modificada ou a mesma string se não tiver nenhum dos prefixos fornecidos</returns>
        public static string RemovePreFix(this string str, StringComparison comparisonType, params string[] preFixes)
        {
            if (str.IsNullOrEmpty()) return str;

            if (preFixes.IsNullOrEmpty()) return str;

            foreach (var preFix in preFixes)
                if (str.StartsWith(preFix, comparisonType))
                    return str.Right(str.Length - preFix.Length);

            return str;
        }

        public static string ReplaceFirst(this string str, string search, string replace,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            var pos = str.IndexOf(search, comparisonType);
            if (pos < 0) return str;

            return str.Substring(0, pos) + replace + str.Substring(pos + search.Length);
        }

        /// <summary>
        ///     Obtém uma substring de uma string do final da string.
        /// </summary>
        public static string Right(this string str, int len)
        {
            if (str.Length < len) throw new Exception("len argument can not be bigger than given string's length!");

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        ///     Usa o método string.Split para dividir determinada string por determinado separador.
        /// </summary>
        public static string[] Split(this string str, string separator) =>
            str.Split(new string[1] { separator }, StringSplitOptions.None);

        /// <summary>
        ///     Usa o método string.Split para dividir determinada string por determinado separador.
        /// </summary>
        public static string[] Split(this string str, string separator, StringSplitOptions options) =>
            str.Split(new string[1] { separator }, options);

        /// <summary>
        ///     Usa o método string.Split para dividir determinada string por <see cref="P:System.Environment.NewLine" />.
        /// </summary>
        public static string[] SplitToLines(this string str) => Split(str, Environment.NewLine);

        /// <summary>
        ///     Usa o método string.Split para dividir determinada string por <see cref="P:System.Environment.NewLine" />.
        /// </summary>
        public static string[] SplitToLines(this string str, StringSplitOptions options) =>
            Split(str, Environment.NewLine, options);

        /// <summary>
        ///     Converte a string PascalCase para a string camelCase.
        /// </summary>
        /// <param name="str">String para converter</param>
        /// <param name="useCurrentCulture">
        ///     definir true para usar a cultura atual. Caso contrário, a cultura invariável será
        ///     usada.
        /// </param>
        /// <param name="handleAbbreviations">defina true para se você deseja converter 'XYZ' para 'xyz'.</param>
        /// <returns>camelCase da string</returns>
        public static string ToCamelCase(this string str, bool useCurrentCulture = false,
            bool handleAbbreviations = false)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            if (str.Length == 1)
            {
                if (!useCurrentCulture) return str.ToLowerInvariant();

                return str.ToLower();
            }

            if (handleAbbreviations && IsAllUpperCase(str))
            {
                if (!useCurrentCulture) return str.ToLowerInvariant();

                return str.ToLower();
            }

            return (useCurrentCulture ? char.ToLower(str[0]) : char.ToLowerInvariant(str[0])) + str.Substring(1);
        }

        /// <summary>
        ///     Converte a string PascalCase/camelCase em sentença (dividindo palavras por espaço).
        ///     Exemplo: "ThisIsSampleSentence" é convertido em "Esta é uma frase de amostra".
        /// </summary>
        /// <param name="str">String para converter.</param>
        /// <param name="useCurrentCulture">
        ///     definir true para usar a cultura atual. Caso contrário, a cultura invariável será
        ///     usada.
        /// </param>
        public static string ToSentenceCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            if (!useCurrentCulture)
                return Regex.Replace(str, "[a-z][A-Z]",
                    m => m.Value[0] + " " + char.ToLowerInvariant(m.Value[1]));

            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }

        /// <summary>
        ///     Converte a string PascalCase/camelCase para kebab-case.
        /// </summary>
        /// <param name="str">String para converter.</param>
        /// <param name="useCurrentCulture">
        ///     definir true para usar a cultura atual. Caso contrário, a cultura invariável será
        ///     usada.
        /// </param>
        public static string ToKebabCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            str = ToCamelCase(str);
            if (!useCurrentCulture)
                return Regex.Replace(str, "[a-z][A-Z]",
                    m => m.Value[0] + "-" + char.ToLowerInvariant(m.Value[1]));

            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + "-" + char.ToLower(m.Value[1]));
        }

        /// <summary>
        ///     Converte a string PascalCase/camelCase dada para snake case.
        ///     Exemplo: "ThisIsSampleSentence" é convertido em "this_is_a_sample_sentence".
        ///     https://github.com/npgsql/npgsql/blob/dev/src/Npgsql/NameTranslation/NpgsqlSnakeCaseNameTranslator.cs#L51
        /// </summary>
        /// <param name="str">String para converter.</param>
        /// <returns></returns>
        public static string ToSnakeCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            var builder = new StringBuilder(str.Length + Math.Min(2, str.Length / 5));
            UnicodeCategory? previousCategory = null;
            for (var currentIndex = 0; currentIndex < str.Length; currentIndex++)
            {
                var currentChar = str[currentIndex];
                if (currentChar == '_')
                {
                    builder.Append('_');
                    previousCategory = null;
                    continue;
                }

                var currentCategory = char.GetUnicodeCategory(currentChar);
                switch (currentCategory)
                {
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                        if (previousCategory == UnicodeCategory.SpaceSeparator ||
                            previousCategory == UnicodeCategory.LowercaseLetter ||
                            (previousCategory != UnicodeCategory.DecimalDigitNumber && previousCategory.HasValue &&
                             currentIndex > 0 && currentIndex + 1 < str.Length && char.IsLower(str[currentIndex + 1])))
                            builder.Append('_');

                        currentChar = char.ToLower(currentChar);
                        break;
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (previousCategory == UnicodeCategory.SpaceSeparator) builder.Append('_');

                        break;
                    default:
                        if (previousCategory.HasValue) previousCategory = UnicodeCategory.SpaceSeparator;

                        continue;
                }

                builder.Append(currentChar);
                previousCategory = currentCategory;
            }

            return builder.ToString();
        }

        /// <summary>
        ///     Converte string em valor enum.
        /// </summary>
        /// <typeparam name="T">Tipo de enum</typeparam>
        /// <param name="value">Valor da string a ser convertida</param>
        /// <returns>Retorna o objeto enum</returns>
        public static T ToEnum<T>(this string value) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        ///     Converte string em valor enum.
        /// </summary>
        /// <typeparam name="T">Tipo de enum</typeparam>
        /// <param name="value">Valor da string a ser convertida</param>
        /// <param name="ignoreCase">Ignorar maiúsculas</param>
        /// <returns>Retorna o objeto enum</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static string ToMd5(this string str)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(str);
                var array = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                var array2 = array;
                foreach (var hashByte in array2) sb.Append(hashByte.ToString("X2"));

                return sb.ToString();
            }
        }

        /// <summary>
        ///     Converte a string camelCase em string PascalCase.
        /// </summary>
        /// <param name="str">String para converter</param>
        /// <param name="useCurrentCulture">
        ///     definir true para usar a cultura atual. Caso contrário, a cultura invariável será
        ///     usada.
        /// </param>
        /// <returns>PascalCase da string</returns>
        public static string ToPascalCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            if (str.Length == 1)
            {
                if (!useCurrentCulture) return str.ToUpperInvariant();

                return str.ToUpper();
            }

            return (useCurrentCulture ? char.ToUpper(str[0]) : char.ToUpperInvariant(str[0])) + str.Substring(1);
        }

        /// <summary>
        ///     Converte a string camelCase em string PascalCase.
        /// </summary>
        /// <param name="str">String para converter</param>
        /// <param name="useCurrentCulture">
        ///     definir true para usar a cultura atual. Caso contrário, a cultura invariável será
        ///     usada.
        /// </param>
        /// <returns>PascalCase da string</returns>
        public static string ToPascalCase(this string input, char splitChar = ' ')
        {
            string[] words = input.Split(splitChar);
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if (word.Length > 1)
                {
                    char firstChar = char.ToUpper(word[0]);
                    string restChars = word.Substring(1).ToLower();
                    words[i] = firstChar + restChars;
                }

                if (word.Length == 1 || word.Length == 2)
                {
                    words[i] = word.ToLower();
                }
            }

            string output = string.Join(splitChar, words);
            return output;
        }

        public static string ToPascalCase(this string input)
        {
            string[] words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if (word.Length > 1)
                {
                    char firstChar = char.ToUpper(word[0]);
                    string restChars = word.Substring(1).ToLower();
                    words[i] = firstChar + restChars;
                }

                if (word.Length == 1 || word.Length == 2)
                {
                    words[i] = word.ToLower();
                }
            }

            string output = string.Join(' ', words);
            return output;
        }

        /// <summary>
        ///     Obtém uma substring de uma string do início da string se exceder o comprimento máximo.
        /// </summary>
        public static string Truncate(this string str, int maxLength)
        {
            if (str == null) return null;

            if (str.Length <= maxLength) return str;

            return str.Left(maxLength);
        }

        /// <summary>
        ///     Obtém uma substring de uma string de Ending da string se ela exceder o comprimento máximo.
        /// </summary>
        public static string TruncateFromBeginning(this string str, int maxLength)
        {
            if (str == null) return null;

            if (str.Length <= maxLength) return str;

            return str.Right(maxLength);
        }

        /// <summary>
        ///     Obtém uma substring de uma string do início da string se exceder o comprimento máximo.
        ///     Adiciona um postfix "..." ao final da string se ela estiver truncada.
        ///     A string de retorno não pode ser maior que maxLength.
        /// </summary>
        public static string TruncateWithPostfix(this string str, int maxLength) =>
            str.TruncateWithPostfix(maxLength, "...");

        /// <summary>
        ///     Obtém uma substring de uma string do início da string se exceder o comprimento máximo.
        ///     Adiciona dado <paramref name="postfix" /> ao final da string se ela estiver truncada.
        ///     A string de retorno não pode ser maior que maxLength.
        /// </summary>
        public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
        {
            if (str == null) return null;

            if (str == string.Empty || maxLength == 0) return string.Empty;

            if (str.Length <= maxLength) return str;

            if (maxLength <= postfix.Length) return postfix.Left(maxLength);

            return str.Left(maxLength - postfix.Length) + postfix;
        }

        /// <summary>
        ///     Converte determinada string em uma matriz de bytes usando <see cref="P:System.Text.Encoding.UTF8" /> encoding.
        /// </summary>
        public static byte[] GetBytes(this string str) => str.GetBytes(Encoding.UTF8);

        /// <summary>
        ///     Converte determinada string em uma matriz de bytes usando o dado <paramref name="encoding" />
        /// </summary>
        public static byte[] GetBytes(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        /// <summary>
        ///     Converte um byte[] em string sem BOM (A marca de ordem de byte (BOM)
        ///     é um caractere Unicode usado para denotar a extremidade (ordem de bytes)
        ///     de um arquivo de texto ou fluxo de dados, cujo código é U+FEFF. Seu uso é opcional e,
        ///     se usado, deve aparecer no começo do fluxo de texto.).
        /// </summary>
        /// <param name="bytes">O byte[] a ser convertido em string</param>
        /// <param name="encoding">A codificação para obter a string. O padrão é UTF8</param>
        /// <returns></returns>
        public static string ConvertFromBytesWithoutBom(byte[] bytes, Encoding encoding = null)
        {
            if (bytes == null) return null;

            if (encoding == null) encoding = Encoding.UTF8;

            if (bytes.Length >= 3 && bytes[0] == 239 && bytes[1] == 187 && bytes[2] == 191)
                return encoding.GetString(bytes, 3, bytes.Length - 3);

            return encoding.GetString(bytes);
        }

        private static bool IsAllUpperCase(string input)
        {
            for (var i = 0; i < input.Length; i++)
                if (char.IsLetter(input[i]) && !char.IsUpper(input[i]))
                    return false;

            return true;
        }

        public static string PorExtenso(this int identificador)
        {
            switch (identificador)
            {
                case 1: return "Um";
                case 2: return "Dois";
                case 3: return "Tres";
                case 4: return "Quatro";
                case 5: return "Cinco";
                case 6: return "Seis";
                case 7: return "Sete";
                case 8: return "Oito";
                case 9: return "Nove";

                case 10: return "Dez";
                case 11: return "Onze";
                case 12: return "Doze";
                case 13: return "Treze";
                case 14: return "Quatorze";
                case 15: return "Quinze";
                case 16: return "Dezesseis";
                case 17: return "Dezessete";
                case 18: return "Dezoito";
                case 19: return "Dezenove";

                case 20: return "Vinte";
                case 30: return "Trinta";
                case 40: return "Quarenta";
                case 50: return "Cinquenta";
                case 60: return "Sessenta";
                case 70: return "Setenta";
                case 80: return "Oitenta";
                case 90: return "Noventa";

                case 100: return "Cem";
                case 200: return "Duzentos";
                case 300: return "Trezentos";
                case 400: return "Quatrocentos";
                case 500: return "Quinhentos";
                case 600: return "Seicentos";
                case 700: return "Setecentos";
                case 800: return "Oitocentos";
                case 900: return "Novecentos";

                default: return "Valor inválido";
            }
        }

        public static string RemoveScape(this string text)
        {
            if (text.IsNullOrWhiteSpace()) return text;
            return text
                .Replace("  ", " ")
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\"", "");
        }

        public static string ToBase64String(this object texto)
        {
            var str = texto.ToString();
            byte[] binario = Convert.FromBase64String(str);
            return Encoding.ASCII.GetString(binario);
        }

        public static string ToStringBase64(this object texto)
        {
            var str = texto.ToString();
            byte[] binario = Encoding.ASCII.GetBytes(str);
            return Convert.ToBase64String(binario);
        }


        private class MathEvalWrapper
        {
            private string _wrapFormat;

            public MathEvalWrapper(string wrapFormat)
            {
                _wrapFormat = wrapFormat;
            }

            public string MatchEval(Match match)
            {
                if (match.Groups[1].Success)
                {
                    return string.Format(_wrapFormat, match);
                }

                return string.Empty;
            }
        }

        //
        // Resumo:
        //     Busca um conjunto de palavras em um texto e marca em um formato específico
        //
        // Parâmetros:
        //   text:
        //     Texto a ser verificado
        //
        //   keywords:
        //     Conjunto de palavras
        //
        //   wrapFormat:
        //     Formato. Ex: "{0}"
        //
        // Devoluções:
        //     Texto com marcações
        public static string HighlightKeywords(this string text, string[] keywords, string wrapFormat)
        {
            if (keywords == null)
            {
                return text;
            }

            if (keywords.Length == 0)
            {
                return text;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            Regex regex = new Regex(string.Format("({0})", string.Join("|", keywords)),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MathEvalWrapper @object = new MathEvalWrapper(wrapFormat);
            return regex.Replace(text, @object.MatchEval);
        }

        //
        // Resumo:
        //     Busca por um texto dentro de outro texto e marca em um formato específico
        //
        // Parâmetros:
        //   text:
        //     Texto a ser verificado
        //
        //   keyword:
        //     Texto a ser encontrado
        //
        //   wrapFormat:
        //     Formato. Ex: "{0}"
        public static string HighlightKeyword(this string text, string keyword, string wrapFormat)
        {
            return text.HighlightKeywords(new string[1] { keyword }, wrapFormat);
        }

        //
        // Resumo:
        //     Filtra somente os caracteres decimais na string
        //
        // Parâmetros:
        //   source:
        //     Texto a ser filtrado

        //
        // Resumo:
        //     Filtra os caracteres decimais e letras na string
        //
        // Parâmetros:
        //   source:
        //     Texto a ser filtrado
        public static string GetDigitsAndLetters(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in source)
            {
                if (char.IsLetterOrDigit(c))
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }

        //
        // Resumo:
        //     Remove os acentos da string
        //
        // Parâmetros:
        //   text:
        //     Texto para remoção de acentos
        public static string RemoveDiacritics(this string text)
        {
            string text2 = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();
            string text3 = text2;
            foreach (char c in text3)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        //
        // Resumo:
        //     Insere uma string dentro de uma máscara
        //
        // Parâmetros:
        //   source:
        //     Texto a ser inserido. Ex:99999999999
        //
        //   mask:
        //     Máscara. Ex: 999.999.999-99
        public static string SetMask(this string source, string mask)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(mask))
                {
                    source = source.GetDigitsAndLetters();
                    List<char> list = new List<char>(mask.ToCharArray());
                    Queue<KeyValuePair<int, char>> queue = new Queue<KeyValuePair<int, char>>();
                    int num = 0;
                    foreach (char item in list)
                    {
                        if (!char.IsLetter(item) & !char.IsNumber(item) & !item.Equals('*'))
                        {
                            queue.Enqueue(new KeyValuePair<int, char>(num, item));
                        }

                        num++;
                    }

                    while (queue.Count > 0)
                    {
                        KeyValuePair<int, char> keyValuePair = queue.Dequeue();
                        source = source.Insert(keyValuePair.Key, keyValuePair.Value.ToString());
                    }

                    return source;
                }

                return source;
            }
            catch
            {
                return source;
            }
        }

        //
        // Resumo:
        //     Coloca no formato Title Case
        //
        // Parâmetros:
        //   source:
        //     Texto
        public static string ToTitleCase(this string source)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(source.ToLower());
            }

            return source;
        }

        //
        // Resumo:
        //     Remove as tags HTML de um texto
        //
        // Parâmetros:
        //   source:
        //     Texto HTML
        //
        // Devoluções:
        //     Texto sem as tags HTML
        public static string StripHtml(this string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        //
        // Parâmetros:
        //   value:
        //
        //   maxlength:
    }

    public class ColoredText
    {
        private bool _bright;
        private int _color;
        private readonly string _message;

        public ColoredText(string message)
        {
            _message = message;
        }

        public ColoredText Bright()
        {
            _bright = true;
            return this;
        }

        public ColoredText Red()
        {
            _color = 31;
            return this;
        }

        public ColoredText Black()
        {
            _color = 30;
            return this;
        }

        public ColoredText Green()
        {
            _color = 32;
            return this;
        }

        public ColoredText Orange()
        {
            _color = 33;
            return this;
        }

        public ColoredText Blue()
        {
            _color = 34;
            return this;
        }

        public ColoredText Purple()
        {
            _color = 35;
            return this;
        }

        public ColoredText Cyan()
        {
            _color = 36;
            return this;
        }

        public ColoredText LightGray()
        {
            _color = 37;
            return this;
        }

        public static implicit operator string(ColoredText t) => t.ToString();

        public override string ToString()
        {
            if (StringExtensions.NoColor || _color == 0) return _message;

            var colorString = _color.ToString();
            if (_bright) colorString += "m\x1B[1";

            return $"\x1B[{colorString}m{_message}\x1B[0m\x1B[39m\x1B[49m";
        }
    }

    public static class StringExtensions
    {
        public static bool NoColor { get; set; }

        public static ColoredText Orange(this string s) => new ColoredText(s).Orange();

        public static ColoredText Black(this string s) => new ColoredText(s).Black();

        public static ColoredText Red(this string s) => new ColoredText(s).Red();

        public static ColoredText Green(this string s) => new ColoredText(s).Green();

        public static ColoredText Blue(this string s) => new ColoredText(s).Blue();

        public static ColoredText Purple(this string s) => new ColoredText(s).Purple();

        public static ColoredText Cyan(this string s) => new ColoredText(s).Cyan();

        public static ColoredText LightGray(this string s) => new ColoredText(s).LightGray();

        public static string TrimStart(this string target, string trimChars) =>
            target.TrimStart(trimChars.ToCharArray());

        public static string TrimEnd(this string target, string trimChars) => target.TrimEnd(trimChars.ToCharArray());

        public static string ToCamelCase(this string str)
        {
            var firstChar = str[0];

            if (firstChar == char.ToLowerInvariant(firstChar)) return str;

            var name = str.ToCharArray();
            name[0] = char.ToLowerInvariant(firstChar);

            return new string(name);
        }

        public static string ToSnakeCase(this string str)
        {
            var builder = new StringBuilder();
            var name = str;
            var previousUpper = false;

            for (var i = 0; i < name.Length; i++)
            {
                var c = name[i];
                if (char.IsUpper(c))
                {
                    if (i > 0 && !previousUpper) builder.Append("_");
                    builder.Append(char.ToLowerInvariant(c));
                    previousUpper = true;
                }
                else
                {
                    builder.Append(c);
                    previousUpper = false;
                }
            }

            return builder.ToString();
        }

        public static string ToI18n(this string chn, string es, string en = null) =>
            Thread.CurrentThread.CurrentUICulture.Name == "es-ES" ? es :
            Thread.CurrentThread.CurrentUICulture.Name == "en-EN" ? en ?? chn : chn;

        public static string Latin1ToGB2312(this string latin1String) => string.IsNullOrWhiteSpace(latin1String)
            ? ""
            : Encoding.GetEncoding("gb2312").GetString(Encoding.GetEncoding("iso-8859-1").GetBytes(latin1String));

        //     public static string GetDescription(this Enum GenericEnum)
        //     {
        //         Type genericEnumType = GenericEnum.GetType();
        //         MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
        //         if ((memberInfo != null && memberInfo.Length > 0))
        //         {
        //             var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
        //             if ((_Attribs != null && _Attribs.Count() > 0))
        //             {
        //                 return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
        //             }
        //         }
        //         return GenericEnum.ToString();
        //     }
        // }

        //     public static class ConsoleExt
        //     {
        //         public static void WriteLineColor(this string s, ConsoleColor foregroundColor)
        //         {
        // #if WINDOWS || Linux
        //             Console.BackgroundColor = ConsoleColor.Black;
        //             Console.ForegroundColor = foregroundColor;
        // #endif
        //             Console.WriteLine(s);
        // #if WINDOWS || Linux
        //            Console.ResetColor();
        // #endif
        //         }
        //
        //         public static void WriteLine(this string s) => s.WriteLineColor(ConsoleColor.White);
        //         public static void WriteLineCyan(string s) => s.WriteLineColor(ConsoleColor.Cyan);
        //         public static void WriteLineGreen(string s) => s.WriteLineColor(ConsoleColor.Green);
        //         public static void WriteLineDarkYellow(string s) => s.WriteLineColor(ConsoleColor.DarkYellow);
        //     }
    }
}