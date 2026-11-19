#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Utils.Extension;

public static class EnumExtensions
{
    public static Type GetTypeFromEnumsName(string name, string pre = "AME.Enums+", string assemblyString = null)
    {
        Type t1;
        if (string.IsNullOrEmpty(assemblyString))
            t1 = Type.GetType(pre + name, false, true);
        else
            t1 = Assembly.Load(assemblyString).GetType(pre + name, false, true);
        if (t1 == null) throw new ArgumentNullException(pre + name);

        return t1;
    }

    public static string[] GetNamesFromEnums(this Type enumType)
    {
        var t1 = Enum.GetNames(enumType);
        return t1;
    }

    public static string[] GetNamesFromEnumsName(string name, string pre = "AME.Enums+", string assemblyString = null)
    {
        var t1 = Enum.GetNames(GetTypeFromEnumsName(name, pre, assemblyString));
        return t1;
    }

    public static Array GetValuesFromEnumsName(string name, string pre = "AME.Enums+", string assemblyString = null)
    {
        var t1 = Enum.GetValues(GetTypeFromEnumsName(name, pre, assemblyString));
        return t1;
    }

    public static IEnumerable<List<string>> GetEnumValueAndDescriptionsFromEnumsName(string name, string pre = "AME.Enums+", string assemblyString = null) => GetTypeFromEnumsName(name, pre, assemblyString).GetEnumValueAndDescriptions();


    public static string GetEnumName(this Enum en) => GetDescription(en);


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


    public static IEnumerable<List<string>> GetEnumValueAndDescriptions(this Type type)
    {
        var descs = new List<List<string>>();
        var values = Enum.GetValues(type);
        foreach (int value in values)
        {
            var name = Enum.GetName(type, value);
            var field = type.GetField(name);
            var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (fds != null && fds.Length > 0)
                foreach (DescriptionAttribute fd in fds)
                    descs.Add(new List<string> { name, fd.Description ?? name, value.ToString() });
            else
                descs.Add(new List<string> { name, name, value.ToString() });
        }

        return descs;
    }

    public static T GetValueFromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
            if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description) return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name == description) return (T)field.GetValue(null);
            }

        throw new ArgumentException("Not found.", nameof(description));
    }

    /// <summary>
    ///     Ex.
    ///     public enum Soda : int
    ///     {
    ///     SevenUp = 0,
    ///     Sprite = 1,
    ///     [Display(Name = "Dr. Pepper")]
    ///     DrPepper = 2
    ///     }
    ///     Soda.DrPepper.GetDisplayName();
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static string GetDisplayName(this Enum enumValue)
    {
        string displayName;
        displayName = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName();
        if (String.IsNullOrEmpty(displayName))
        {
            displayName = enumValue.ToString();
        }

        return displayName;
    }
}
