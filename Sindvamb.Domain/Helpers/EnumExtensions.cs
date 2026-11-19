using System.ComponentModel;

namespace Sindvamb.Domain.Helpers;

public static class EnumExtensions
{
    public static string GetDescription(this Enum en)
    {
        var temType = en.GetType();
        var memberInfos = temType.GetMember(en.ToString());
        if (memberInfos != null && memberInfos.Length > 0)
        {
            var objs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs != null && objs.Length > 0) return ((DescriptionAttribute)objs[0]).Description;
        }

        return en.ToString();
    }
}
