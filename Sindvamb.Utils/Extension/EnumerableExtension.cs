#region

using System.ComponentModel;

#endregion

namespace Sindvamb.Utils.Extension;

public static class EnumerableExtension
{
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        var knownKeys = new HashSet<TKey>();
        foreach (var element in source)
            if (knownKeys.Add(keySelector(element)))
                yield return element;
    }

    public static string GetEnumDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());

        var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if (attributes != null && attributes.Any()) return attributes.First().Description;

        return value.ToString();
    }
}
