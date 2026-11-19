#region

using System.ComponentModel;
using System.Reflection;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Utils.Extension;

public static class DetailCompareExtension
{
    public static List<PropChange> DetailedCompare<T>(this T val1, T val2)
    {
        var changes = new List<PropChange>();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(pi => pi.GetCustomAttributes(typeof(IsDetailedComparedPropAttribute), true).Length > 0).ToArray();

        foreach (var property in properties)
        {
            var propName = property.Name;
            try
            {
                var attribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single();
                propName = attribute.DisplayName;
            }
            catch (Exception ex)
            {
            }

            var propCustomType = property.PropertyType.Name;
            try
            {
                var attribute = property.GetCustomAttributes(typeof(CustomTypeAttribute), true).Cast<CustomTypeAttribute>().Single();
                propCustomType = attribute.Type;
            }
            catch (Exception ex)
            {
            }

            var propChange = new PropChange
            {
                Property = propName,
                OldValue = property.GetValue(val1),
                NewValue = property.GetValue(val2),
                Type = propCustomType
            };

            if (propChange.OldValue == null && propChange.NewValue == null) continue;

            if ((propChange.OldValue == null && propChange.NewValue != null) || (propChange.OldValue != null && propChange.NewValue == null)
               )
            {
                changes.Add(propChange);
                continue;
            }

            if (!Equals(propChange.OldValue, propChange.NewValue)) changes.Add(propChange);
        }

        return changes;
    }
}

public class PropChange
{
    public string Property { get; set; }
    public string Type { get; set; }
    public object OldValue { get; set; }
    public object NewValue { get; set; }
}

public class IsDetailedComparedPropAttribute : Attribute
{
}

public class CustomTypeAttribute : Attribute
{
    public CustomTypeAttribute(ECustomTypeAttribute _type)
    {
        Type = _type.ToString();
    }

    public string Type { get; set; }
}

public enum ECustomTypeAttribute
{
    PolicyStatus,
    DateTime
}
