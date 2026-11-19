

using System.Collections;

namespace Sindvamb.Web.Shared.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable Append(this IEnumerable first, params object[] second) => first.OfType<object>().Concat(second);

    public static IEnumerable<T> Append<T>(this IEnumerable<T> first, params T[] second) => first.Concat(second);

    public static IEnumerable Prepend(this IEnumerable first, params object[] second) => second.Concat(first.OfType<object>());

    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> first, params T[] second) => second.Concat(first);
}

public static class ArrayExtensions
{
    public static object[] Append(this object[] first, params object[] second) => second == null ? first : first.ToList().OfType<object>().Concat(second.ToList()).ToArray();

    public static T[] Append<T>(this T[] first, params T[] second) => second == null ? first : first == null ? null : first.ToList().Concat(second.ToList()).ToArray();
}
