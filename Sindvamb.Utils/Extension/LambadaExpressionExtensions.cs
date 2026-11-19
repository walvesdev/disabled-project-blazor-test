#region

using System.Linq.Expressions;

#endregion

namespace Sindvamb.Utils.Extension;

public static class LambadaExpression
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> True<T>() => p => true;

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> False<T>() => p => false;

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static Expression<Func<T, TKey>> GetOrderExpression<T, TKey>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CreateEqual<T>(string propertyName, string propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue);
        return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CreateNotEqual<T>(string propertyName, string propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p"); //创建参数p
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue); //创建常数
        return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
    }

    /// <summary>
    ///     创建lambda表达式：p=>p.propertyName > propertyValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CreateGreaterThan<T>(string propertyName, string propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p"); //创建参数p
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue); //创建常数
        return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
    }

    /// <summary>
    ///     创建lambda表达式：p=>p.propertyName [ propertyValue]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CreateLessThan<T>(string propertyName, string propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p"); //创建参数p
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue); //创建常数
        return Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameter);
    }

    /// <summary>
    ///     创建lambda表达式：p=>p.propertyName >= propertyValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CreateGreaterThanOrEqual<T>(string propertyName, string propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p"); //创建参数p
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue); //创建常数
        return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameter);
    }

    /// <summary>
    ///     创建lambda表达式：p=>p.propertyName = propertyValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CreateLessThanOrEqual<T>(string propertyName, string propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p"); //创建参数p
        var member = Expression.PropertyOrField(parameter, propertyName);
        var constant = Expression.Constant(propertyValue); //创建常数
        return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
    }

    /// <summary>
    ///     创建lambda表达式：p=>p.propertyName.Contains(propertyValue)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> GetContains<T>(string propertyName, string propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var constant = Expression.Constant(propertyValue, typeof(string));
        return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
    }

    /// <summary>
    ///     创建lambda表达式：!(p=>p.propertyName.Contains(propertyValue))
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> GetNotContains<T>(string propertyName, string propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var member = Expression.PropertyOrField(parameter, propertyName);
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var constant = Expression.Constant(propertyValue, typeof(string));
        return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(member, method, constant)), parameter);
    }
}
