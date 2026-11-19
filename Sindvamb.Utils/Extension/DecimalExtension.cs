#region

using System.Globalization;

#endregion

namespace Sindvamb.Utils.Extension;

public static class DecimalExtension
{
    public static decimal ToDecimal(this string val)
    {
        var cultureInfo = "pt-BR";
        var culture = new CultureInfo(cultureInfo);
        var valid = decimal.TryParse(val, NumberStyles.AllowCurrencySymbol | NumberStyles.Number, culture, out var result);
        return valid ? result : 0;
    }

    public static string ToDescription(this decimal valor)
    {
        if ((valor <= 0) | (valor >= 1000000000000000))
        {
            return string.Empty;
        }

        var strValor = valor.ToString("000000000000000.00");
        var valorPorExtenso = string.Empty;
        for (var i = 0; i <= 15; i += 3)
        {
            valorPorExtenso += WriteValueToString(Convert.ToDecimal(strValor.Substring(i, 3)));
            if ((i == 0) & (valorPorExtenso != string.Empty))
            {
                if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                    valorPorExtenso += " trilhão" + (Convert.ToDecimal(strValor.Substring(3, 12)) > 0 ? " e " : string.Empty);
                else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1) valorPorExtenso += " trilhões" + (Convert.ToDecimal(strValor.Substring(3, 12)) > 0 ? " e " : string.Empty);
            }
            else if ((i == 3) & (valorPorExtenso != string.Empty))
            {
                if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                    valorPorExtenso += " bilhão" + (Convert.ToDecimal(strValor.Substring(6, 9)) > 0 ? " e " : string.Empty);
                else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1) valorPorExtenso += " bilhões" + (Convert.ToDecimal(strValor.Substring(6, 9)) > 0 ? " e " : string.Empty);
            }
            else if ((i == 6) & (valorPorExtenso != string.Empty))
            {
                if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                    valorPorExtenso += " milhão" + (Convert.ToDecimal(strValor.Substring(9, 6)) > 0 ? " e " : string.Empty);
                else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1) valorPorExtenso += " milhões" + (Convert.ToDecimal(strValor.Substring(9, 6)) > 0 ? " e " : string.Empty);
            }
            else if ((i == 9) & (valorPorExtenso != string.Empty))
            {
                if (Convert.ToInt32(strValor.Substring(9, 3)) > 0) valorPorExtenso += " mil" + (Convert.ToDecimal(strValor.Substring(12, 3)) > 0 ? " e " : string.Empty);
            }

            if (i == 12)
            {
                if (valorPorExtenso.Length > 8)
                {
                    if ((valorPorExtenso.Substring(valorPorExtenso.Length - 6, 6) == "bilhão") | (valorPorExtenso.Substring(valorPorExtenso.Length - 6, 6) == "milhão"))
                        valorPorExtenso += " de";
                    else if ((valorPorExtenso.Substring(valorPorExtenso.Length - 7, 7) == "bilhões") | (valorPorExtenso.Substring(valorPorExtenso.Length - 7, 7) == "milhões") | (valorPorExtenso.Substring(valorPorExtenso.Length - 8, 7) == "trilhões"))
                        valorPorExtenso += " de";
                    else if (valorPorExtenso.Substring(valorPorExtenso.Length - 8, 8) == "trilhões") valorPorExtenso += " de";
                }

                //if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                //    valorPorExtenso += " real";
                //else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                //    valorPorExtenso += " reais";
                if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valorPorExtenso != string.Empty) valorPorExtenso += " e ";
            }
            //if (i == 15)
            //    if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
            //        valorPorExtenso += " centavo";
            //    else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
            //        valorPorExtenso += " centavos";
        }

        return valorPorExtenso;
    }

    private static string WriteValueToString(decimal valor)
    {
        if (valor <= 0) return string.Empty;

        var montagem = string.Empty;
        if ((valor > 0) & (valor < 1)) valor *= 100;
        var strValor = valor.ToString("000");
        var a = Convert.ToInt32(strValor.Substring(0, 1));
        var b = Convert.ToInt32(strValor.Substring(1, 1));
        var c = Convert.ToInt32(strValor.Substring(2, 1));
        if (a == 1)
            montagem += b + c == 0 ? "cem" : "cento";
        else if (a == 2)
            montagem += "duzentos";
        else if (a == 3)
            montagem += "trezentos";
        else if (a == 4)
            montagem += "quatrocentos";
        else if (a == 5)
            montagem += "quinhentos";
        else if (a == 6)
            montagem += "seiscentos";
        else if (a == 7)
            montagem += "setecentos";
        else if (a == 8)
            montagem += "oitocentos";
        else if (a == 9) montagem += "novecentos";

        if (b == 1)
        {
            if (c == 0)
                montagem += (a > 0 ? " e " : string.Empty) + "dez";
            else if (c == 1)
                montagem += (a > 0 ? " e " : string.Empty) + "onze";
            else if (c == 2)
                montagem += (a > 0 ? " e " : string.Empty) + "doze";
            else if (c == 3)
                montagem += (a > 0 ? " e " : string.Empty) + "treze";
            else if (c == 4)
                montagem += (a > 0 ? " e " : string.Empty) + "quatorze";
            else if (c == 5)
                montagem += (a > 0 ? " e " : string.Empty) + "quize";
            else if (c == 6)
                montagem += (a > 0 ? " e " : string.Empty) + "dezesseis";
            else if (c == 7)
                montagem += (a > 0 ? " e " : string.Empty) + "dezessete";
            else if (c == 8)
                montagem += (a > 0 ? " e " : string.Empty) + "dezoito";
            else if (c == 9) montagem += (a > 0 ? " e " : string.Empty) + "dezenove";
        }
        else if (b == 2)
        {
            montagem += (a > 0 ? " e " : string.Empty) + "vinte";
        }
        else if (b == 3)
        {
            montagem += (a > 0 ? " e " : string.Empty) + "trinta";
        }
        else if (b == 4)
        {
            montagem += (a > 0 ? " e " : string.Empty) + "quarenta";
        }
        else if (b == 5)
        {
            montagem += (a > 0 ? " e " : string.Empty) + "cinquenta";
        }
        else if (b == 6)
        {
            montagem += (a > 0 ? " e " : string.Empty) + "sessenta";
        }
        else if (b == 7)
        {
            montagem += (a > 0 ? " e " : string.Empty) + "setenta";
        }
        else if (b == 8)
        {
            montagem += (a > 0 ? " e " : string.Empty) + "oitenta";
        }
        else if (b == 9)
        {
            montagem += (a > 0 ? " e " : string.Empty) + "noventa";
        }

        if ((strValor.Substring(1, 1) != "1") & (c != 0) & (montagem != string.Empty)) montagem += " e ";

        if (strValor.Substring(1, 1) != "1")
        {
            if (c == 1)
                montagem += "um";
            else if (c == 2)
                montagem += "dois";
            else if (c == 3)
                montagem += "três";
            else if (c == 4)
                montagem += "quatro";
            else if (c == 5)
                montagem += "cinco";
            else if (c == 6)
                montagem += "seis";
            else if (c == 7)
                montagem += "sete";
            else if (c == 8)
                montagem += "oito";
            else if (c == 9) montagem += "nove";
        }

        return montagem;
    }
}
