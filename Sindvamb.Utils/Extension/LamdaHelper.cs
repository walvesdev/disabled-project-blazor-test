#region

using System.Linq.Expressions;

#endregion

namespace Sindvamb.Utils.Extension;

/// <summary>
/// </summary>
public class LamdaHelper
{
    public static MemberExpression GetExpression<T, TKey>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        return Expression.Property(parameter, propertyName);
    }


    public static Expression<Func<T, bool>> True<T>() => p => true;


    public static Expression<Func<T, bool>> False<T>() => p => false;


    public static Expression<Func<T, TKey>> GetOrderExpression<T, TKey>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
    }


    public static Expression<Func<T, bool>> CreateEqual<T, S>(string propertyName, S propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue, typeof(S));
        return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
    }


    public static Expression<Func<T, bool>> CreateNotEqual<T, S>(string propertyName, S propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue, typeof(S));
        return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
    }


    public static Expression<Func<T, bool>> CreateGreaterThan<T, S>(string propertyName, S propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue, typeof(S));
        return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
    }


    public static Expression<Func<T, bool>> CreateLessThan<T, S>(string propertyName, S propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue, typeof(S));
        return Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameter);
    }


    public static Expression<Func<T, bool>> CreateGreaterThanOrEqual<T, S>(string propertyName, S propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue, typeof(S));
        return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameter);
    }


    public static Expression<Func<T, bool>> CreateLessThanOrEqual<T, S>(string propertyName, S propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue, typeof(S));
        return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
    }


    public static Expression<Func<T, bool>> GetContains<T>(string propertyName, string propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var constant = Expression.Constant(propertyValue, typeof(string));
        return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
    }


    public static Expression<Func<T, bool>> GetNotContains<T>(string propertyName, string propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var constant = Expression.Constant(propertyValue, typeof(string));
        return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(member, method, constant)), parameter);
    }
}
