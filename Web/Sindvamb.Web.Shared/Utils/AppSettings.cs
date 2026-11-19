namespace Sindvamb.Web.Shared.Utils
{
    public enum AppEnvironment
    {
        DEBUG,
        HML,
        STAGING,
        PRODUCTION
    }


    public static class AppSettings
    {
        public static AppEnvironment Environment { get; set; } = AppEnvironment.DEBUG;

       public static string Version { get; set; } = "1.0.0";

        private static string BaseAddress { get; set; }

        public static Guid SindvamPartnerUnitId { get; set; } = Guid.Parse("032c6ea0-a79f-420e-930b-bc527aaa47d6");

        public static Guid ApplicationsId { get; set; } = Guid.Parse("da1ab9a7-e11c-4a50-b4a3-1307fa8c527a");

        private static string WebPathString { get; set; }

        private static string HttpPathString { get; set; }

        public static bool IsWebServer { get; set; } = false;
        public static string FrontEndUrl => AppServices.IsDevelopment ? "https://localhost:7078" : "https://wwww.sindvamb.com.br";

        public static string GetBaseAddress()
        {
            //switch (Environment) 
            //{
            //    case AppEnvironment.DEBUG: 
            //        BaseAddress = @"https://localhost:5001/api/";
            //        //BaseAddress = @"https://api.sindvamb.com.br/api/";
            //        break;
            //    case AppEnvironment.HML:
            //        BaseAddress = @"https://api.sindvamb.com.br/api/";
            //        break;
            //    case AppEnvironment.STAGING:
            //        BaseAddress = @"https://api.sindvamb.com.br/api/";
            //        break;
            //    case AppEnvironment.PRODUCTION:
            //        BaseAddress = @"https://api.sindvamb.com.br/api/";
            //        break; 
            //    default:
            //        BaseAddress = @"https://api.sindvamb.com.br/api/";
            //        break;
            //}

            BaseAddress = @"https://api.sindvamb.com.br/api/";

#if DEBUG
            BaseAddress = @"https://localhost:5001/api/";
#endif
            

            return BaseAddress;
        }

        public static string GetWebPathString()
        {
            switch (Environment)
            {
                case AppEnvironment.DEBUG:
                    WebPathString = "";
                    break;
                case AppEnvironment.HML:
                    WebPathString = "";
                    break;
                case AppEnvironment.STAGING:
                    WebPathString = "";
                    break;
                case AppEnvironment.PRODUCTION:
                    WebPathString = "";
                    break;
                default:
                    WebPathString = "";
                    break;
            }

            return WebPathString;
        }

        public static string GetApiPathString()
        {
            switch (Environment)
            {
                case AppEnvironment.DEBUG:
                    HttpPathString = "api/";
                    break;
                case AppEnvironment.HML:
                    HttpPathString = "api/";
                    break;
                case AppEnvironment.STAGING:
                    HttpPathString = "api/";
                    break;
                case AppEnvironment.PRODUCTION:
                    HttpPathString = "api/";
                    break;
                default:
                    HttpPathString = "api/";
                    break;
            }

            return HttpPathString;
        }

        public static string GetPathFile()
        {
            switch (Environment)
            {
                case AppEnvironment.DEBUG:
                    return @"../../Arquivos";
                case AppEnvironment.HML:
                    return @"E:\hrbp-arquivos";
                case AppEnvironment.STAGING:
                    return @"";
                case AppEnvironment.PRODUCTION:
                    return @"";
                default:
                    return @"";
            }
        }
    }


}
