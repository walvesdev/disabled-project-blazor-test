#region

using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Utils.Extension;

public static class IQueryableExtensions
{
    public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string property, bool descending) where TSource : class
    {
        var param = Expression.Parameter(typeof(TSource), "c");
        var pi = typeof(TSource).GetProperty(property);
        var selector = Expression.MakeMemberAccess(param, pi);
        var le = Expression.Lambda(selector, param);
        var methodName = descending ? "OrderByDescending" : "OrderBy";
        var resultExp = Expression.Call(typeof(Queryable), methodName, new[] { typeof(TSource), pi.PropertyType }, source.Expression, le);
        return source.Provider.CreateQuery<TSource>(resultExp);
    }


    public static IEnumerable<DataRow> DataTableParallelForEach<T>(DataTable dt)
    {
        var temporaryEnumerable = dt.Rows.Cast<DataRow>();
        return temporaryEnumerable;
    }


    public static List<T> CustomSort<T>(string property, bool ascSort, List<T> itemList = null, BindingList<T> bindingList = null) where T : class
    {
        try
        {
            var source = (itemList ?? bindingList.ToList()).AsQueryable();
            itemList = source.OrderBy(property, !ascSort).ToList();
        }
        catch
        {
        }

        return itemList;
    }

    public static List<T> CustomSort<T>(string property, bool ascSort, BindingList<T> bindingList) where T : class
    {
        List<T> itemList;
        try
        {
            var source = bindingList.ToList().AsQueryable();
            itemList = source.OrderBy(property, !ascSort).ToList();
            return itemList;
        }
        catch
        {
            return bindingList.ToList();
        }
    }
}
