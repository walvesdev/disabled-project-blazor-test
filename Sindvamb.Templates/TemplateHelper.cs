using System.Reflection;

using Microsoft.CodeAnalysis;

using RazorLight;

using Sindvamb.Templates.Pages.Usuarios;

namespace Sindvamb.Templates
{
    public enum TemplateKey
    {
        QRCODEPDFPAGE = 1,
        WELLCOMEPAGE = 2,
        ORDERCREATE = 3,
        RECOVERYPASSWORD = 4,
        REGISTRATIONREQUEST = 5,
        REGISTRATIONREQUESTNOTIFY = 6,
        REGISTRATIONREQUESTDENY = 7,
        REGISTRATIONREQUESTAPPROVED = 8,
        REQUESTCONTACT = 9,
        REQUESTCONTACTNOTIFY = 10,
        REPLYCONTACTREQUEST= 11,
        RECOVERYPASSWORDREQUEST= 12,
        DECLARACAOSESC,
        DECLARACAOASSOCIACAO
    }

    public static class TemplateHelper
    {
        //public static string QRCODEPDFPAGE = "Sindvamb.Templates.Pages.QrCodePDFPages.QrCodePDFPage.cshtml";
        //public static string WELLCOMEPAGE = "Sindvamb.Templates.Pages.Usuarios.Wellcome.cshtml";
        //public static string ORDERCREATE = "Sindvamb.Templates.Pages.Demandas.OrderCreate.cshtml";
        //public static string RECOVERYPASSWORD = "Sindvamb.Templates.Pages.Usuarios.RecoveryPassword.cshtml";
        //public static string REGISTRATIONREQUEST = "Sindvamb.Templates.Pages.Usuarios.RegistrationRequestEmail.cshtml";
        //public static string REGISTRATIONREQUESTNOTIFY = "Sindvamb.Templates.Pages.Usuarios.RegistrationRequestEmailNotify.cshtml";
        //public static string REGISTRATIONREQUESTDENY = "Sindvamb.Templates.Pages.Usuarios.RegistrationRequestEmailDeny.cshtml";

        private static List<KeyValuePair<int, string>> TemaplateList = new()
        {
            new KeyValuePair<int, string>((int)TemplateKey.QRCODEPDFPAGE, "Sindvamb.Templates.Pages.QrCodePDFPages.QrCodePDFPage.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.WELLCOMEPAGE, "Sindvamb.Templates.Pages.Usuarios.Wellcome.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.ORDERCREATE, "Sindvamb.Templates.Pages.Demandas.OrderCreate.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.RECOVERYPASSWORD, "Sindvamb.Templates.Pages.Usuarios.RecoveryPassword.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.REGISTRATIONREQUEST, "Sindvamb.Templates.Pages.Usuarios.RegistrationRequestEmail.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.REGISTRATIONREQUESTNOTIFY, "Sindvamb.Templates.Pages.Usuarios.RegistrationRequestEmailNotify.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.REGISTRATIONREQUESTDENY, "Sindvamb.Templates.Pages.Usuarios.RegistrationRequestEmailDeny.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.REGISTRATIONREQUESTAPPROVED, "Sindvamb.Templates.Pages.Usuarios.RegistrationRequestEmailApproved.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.REQUESTCONTACT, "Sindvamb.Templates.Pages.Usuarios.RequestContact.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.REQUESTCONTACTNOTIFY, "Sindvamb.Templates.Pages.Usuarios.RequestContactNotify.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.REPLYCONTACTREQUEST, "Sindvamb.Templates.Pages.Usuarios.ReplyRequestContact.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.RECOVERYPASSWORDREQUEST, "Sindvamb.Templates.Pages.Usuarios.RecoveryPasswordRequest.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.DECLARACAOASSOCIACAO, "Sindvamb.Templates.Pages.Usuarios.DeclaracaoAssociacao.cshtml"),
            new KeyValuePair<int, string>((int)TemplateKey.DECLARACAOSESC, "Sindvamb.Templates.Pages.Usuarios.DeclaracaoSesc.cshtml"),
        };

        public static async Task<string> GetHtml(TemplateKey templateKey, dynamic model)
        {
            try
            {
                var templatePair = TemaplateList.FirstOrDefault(t => t.Key == (int)templateKey);

                if (!templatePair.Equals(default(KeyValuePair<string, string>)))
                {
                    var template = templatePair.Value;
                    var assemblyFolder = Path.GetDirectoryName(Assembly.GetAssembly(typeof(TemplateHelper)).Location);
                    var metadataReference = MetadataReference.CreateFromFile(Path.Combine(assemblyFolder, "Sindvamb.Templates.dll"));
                    //var resourceNames = Assembly.GetAssembly(typeof(TemplateHelper)).GetManifestResourceNames();

                    var engine = new RazorLightEngineBuilder()
                        .UseMemoryCachingProvider()
                        .UseEmbeddedResourcesProject(Assembly.GetAssembly(typeof(TemplateHelper)))
                        .AddMetadataReferences(metadataReference)
                        .Build();

                    string result = await engine.CompileRenderAsync(template, model);

                    return result;
                }

                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}