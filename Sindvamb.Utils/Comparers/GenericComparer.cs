
using System.Collections;

using Sindvamb.Utils.Comparers;
using Sindvamb.Utils.Extension;


namespace Sindvamb.Utils.Comparers;

public class GenericComparer<T> : IEqualityComparer<T> where T : class
{
    private Func<T, object> _expr { get; }

    public GenericComparer() => _expr = null;

    public GenericComparer(Func<T, object> expr) => _expr = expr;

    public bool Equals(T x, T y)
    {
        var first = _expr?.Invoke(x) ?? x;
        var sec = _expr?.Invoke(y) ?? y;

        if (ObjEquals(first, sec))
            return true;

        var typeProperties = typeof(T).GetProperties();

        foreach (var prop in typeProperties)
        {
            var firstPropVal = prop.GetValue(first, null);
            var secPropVal = prop.GetValue(sec, null);

            if (!ObjEquals(firstPropVal, secPropVal))
            {
                var propType = prop.PropertyType;

                if (IsEnumerableType(propType) && firstPropVal is IEnumerable && !ArrayEquals(firstPropVal, secPropVal))
                    return false;

                if (propType.IsClass)
                {
                    if (!DeepEqualsFromObj(firstPropVal, secPropVal, propType))
                        return false;

                    if (!DeepObjEquals(firstPropVal, secPropVal))
                        return false;
                }
            }
        }

        return true;
    }

    public int GetHashCode(T obj) =>
        _expr?.Invoke(obj).GetHashCode() ?? obj.GetHashCode();

    private bool DeepObjEquals(object x, object y) =>
        new GenericComparer<object>().Equals(x, y);

    private bool DeepEquals<U>(U x, U y) where U : class =>
        new GenericComparer<U>().Equals(x, y);

    private bool DeepEqualsFromObj(object x, object y, Type type)
    {
        dynamic a = Convert.ChangeType(x, type);
        dynamic b = Convert.ChangeType(y, type);
        return DeepEquals(a, b);
    }

    private bool IsEnumerableType(Type type) =>
        type.GetInterface(nameof(IEnumerable)) != null;

    private bool ObjEquals(object x, object y)
    {
        if (x == null && y == null) return true;
        return x != null && x.Equals(y);
    }

    private bool ArrayEquals(object x, object y)
    {
        var firstList = new List<object>((IEnumerable<object>)x);
        var secList = new List<object>((IEnumerable<object>)y);

        if (!firstList.HasSameLengthThan(secList))
            return false;

        var elementType = firstList?.FirstOrDefault()?.GetType();
        int cpt = 0;
        foreach (var e in firstList)
        {
            if (!DeepEqualsFromObj(e, secList[cpt++], elementType))
                return false;
        }

        return true;
    }
}
