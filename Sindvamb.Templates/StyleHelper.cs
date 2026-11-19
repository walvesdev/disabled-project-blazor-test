namespace Sindvamb.Templates
{
    public static class StyleHelper
    {
        public const string Row = "display: flex; flex-direction: column; flex-wrap: wrap; color: #212529; background-color: #fff; width: 100%;";
        public const string Block = "display: block; color: #212529; background-color: #fff; padding: 10px;";
        public const string Col_Sm_4 = "flex: 0 0 33.333333%; max-width: 33.333333%; position: relative; width: 100%;flex-wrap: wrap;";
        public const string Col_Sm_6 = "flex: 0 0 50%; max-width: 50%; position: relative; width: 100%;flex-wrap: wrap;";
        public const string Col_Sm_12 = "flex: 0 0 100%; max-width: 100%; position: relative; width: 100%;flex-wrap: wrap;";
        public const string Hr = "box-sizing: content-box; height: 0; overflow: visible; margin-top: 1rem; margin-bottom: 1rem; border: 0; border-top-color: currentcolor; border-top-style: none; border-top-width: 0px; border-top: 1px solid rgba(0, 0, 0, .1);";
        public const string Btn_Primary = "background-color: #004A8D; color: #FFFFFF;  border-radius: 15px; text-decoration: none; padding-top:15px; padding-bottom:15px; padding-left:10px;padding-right:10px; cursor: pointer;";

        public const string Default = "background-color: #FFFFFF;";

        public static string GetBackgroundForTheme(string cssClassName)
        {
            switch (cssClassName?.Trim().ToLower())
            {
                case "default":
                    return Default;

                default:
                    return Default;
            }
        }
    }
}
