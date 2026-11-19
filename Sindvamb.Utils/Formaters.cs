using Sindvamb.Utils.Extension;

namespace Sindvamb.Utils
{
    /* 
* Formatar CNPJ, CPF e Retirar a Formatação
* 
* Visite nossa página http://www.codigoexpresso.com.br
* 
* by Antonio Azevedo
*  
* public static string FormatCNPJ(string CNPJ)
*        Formata uma string CNPJ 
*        Informar uma string sem formatacao com o codigo do CNPJ 
*        Exemplo '11222333000181' 
*
* public static string FormatCPF(string CPF)
*        Formata uma string CPF 
*        Informar uma string sem formatacao com o codigo do CPF 
*        Exemplo '01122233344'
*        
* public static string SemFormatacao(string Codigo)
*        Retira formatacao de uma string CNPJ ou CPF 
*        Informar uma string formatada com o codigo do CNPJ ou CPF 
*        Exemplo '99.999.999/9999-99' ou '111.222.333-44'
*        
*/
    using System;

    public static class Formaters
    {

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="cpfCnpj">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>

        public static string FormatCpfCnpj(string? cpfCnpj)
        {
            try
            {
                if (cpfCnpj!.IsNullOrWhiteSpace())
                {
                    return string.Empty;
                }

                cpfCnpj = new string(cpfCnpj?.Where(char.IsDigit)?.ToArray());

                if (cpfCnpj.Length > 11)
                {
                    return Convert.ToUInt64(cpfCnpj).ToString(@"00\.000\.000\/0000\-00");
                }
                else
                {
                    return Convert.ToUInt64(cpfCnpj).ToString(@"000\.000\.000\-00");
                }
            }
            catch
            {
                return cpfCnpj;
            }
        }
        /// <summary>
        /// Retira a Formatacao de uma string CNPJ/CPF
        /// </summary>
        /// <param name="codigo">string Codigo Formatada</param>
        /// <returns>string sem formatacao</returns>
        /// <example>Recebe '99.999.999/9999-99' Devolve '99999999999999'</example>

        public static string SemFormatacao(string codigo)
        {
            try
            {
                if (codigo!.IsNullOrWhiteSpace())
                {
                    return string.Empty;
                }

                codigo = new string(codigo?.Where(char.IsDigit)?.ToArray());
                return codigo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            }
            catch
            {
                return codigo;
            }
        }

        public static string FormatCEP(string codigo)
        {
            try
            {
                if (codigo!.IsNullOrWhiteSpace())
                {
                    return string.Empty;
                }

                codigo = new string(codigo?.Where(char.IsDigit)?.ToArray());
                return Convert.ToUInt64(codigo).ToString(@"00\.000\-000");
            }
            catch
            {
                return codigo;
            }
        }

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="code">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>

        public static string FormatCellPhone(string? code)
        {
            try
            {
                if (code!.IsNullOrWhiteSpace())
                {
                    return string.Empty;
                }

                if (code![0] == 0)
                {
                    code = code[1..];
                }

                code = new string(code?.Where(char.IsDigit)?.ToArray());
                return Convert.ToUInt64(code).ToString("(##) #####-####");
            }
            catch 
            {
                return code;
            }
        }

        public static string FormatHomePhone(string? code)
        {
            try
            {
                if (code!.IsNullOrWhiteSpace())
                {
                    return string.Empty;
                }

                if (code![0] == 0)
                {
                    code = code[1..];
                }

                code = new string(code?.Where(char.IsDigit)?.ToArray());
                return Convert.ToUInt64(code).ToString("(##) ####-####");
            }
            catch 
            {
                return code;
            }
        }
    }
}
