
using Sindvamb.Web.Shared.Utils;

using System.Text;



namespace Sindvamb.Web.Shared.Extensions;

/// <summary>
///     Métodos de extensão para coleções.
/// </summary>
public static class CollectionExtensions
{
    // <summary>
    /// Coverte um array em string.
    /// </summary>
    public static string ToStringArray<T>(this T[] array, string delimiter) =>
        // edit: let's replace this with a "better" version using a StringBuilder
        //return (array == null) ? null : (array.Length == 0) ? string.Empty : array.Skip(1).Aggregate(array[0].ToString(), (s, i) => s + "," + i.ToString());
        array == null ? null : array.Length == 0 ? string.Empty : array.Skip(1).Aggregate(new StringBuilder(array[0].ToString()), (s, i) => s.Append(delimiter).Append(i), s => s.ToString());


    public static bool HasSameLengthThan<T>(this IEnumerable<T> list, IEnumerable<T> expected)
    {
        if (list.IsNullOrEmptyCollection() && expected.IsNullOrEmptyCollection())
            return true;

        if ((list.IsNullOrEmptyCollection() && !expected.IsNullOrEmptyCollection()) || (!list.IsNullOrEmptyCollection() && expected.IsNullOrEmptyCollection()))
            return false;

        return list.Count() == expected.Count();
    }

    /// <summary>
    ///     Used to find out if a collection is empty or if it contains no elements.
    /// </summary>
    /// <typeparam name="T">Type of the collection's items.</typeparam>
    /// <param name="list">Collection of items to test.</param>
    /// <returns><c>true</c> if the collection is <c>null</c> or empty (without items), <c>false</c> otherwise.</returns>
    public static bool IsNullOrEmptyCollection<T>(this IEnumerable<T> list) => list == null || !list.Any();

    /// <summary>
    ///     Verifica qualquer objeto de coleção fornecido é nulo ou não tem item.
    /// </summary>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
    {
        if (source != null) return !source.Any();
        return true;
    }

    /// <summary>
    ///     Adiciona um item à coleção se ainda não estiver na coleção.
    /// </summary>
    /// <param name="source">A coleção</param>
    /// <param name="item">Item para verificar e adicionar</param>
    /// <typeparam name="T">Tipo dos itens na coleção</typeparam>
    /// <returns>Retorna True se adicionado, retorna False se não for.</returns>
    public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
    {
        Check.NotNull(source, "source is null");

        if (source.Contains(item)) return false;
        source.Add(item);
        return true;
    }

    /// <summary>
    ///     Adiciona itens à coleção que ainda não estão na coleção.
    /// </summary>
    /// <param name="source">A coleção</param>
    /// <param name="items">Item para verificar e adicionar</param>
    /// <typeparam name="T">Tipo dos itens na coleção</typeparam>
    /// <returns>Retorna os itens adicionados.</returns>
    public static IEnumerable<T> AddIfNotContains<T>(this ICollection<T> source, IEnumerable<T> items)
    {
        Check.NotNull(source, "source is null");

        var addedItems = new List<T>();
        foreach (var item in items)
            if (!source.Contains(item))
            {
                source.Add(item);
                addedItems.Add(item);
            }

        return addedItems;
    }

    /// <summary>
    ///     Adiciona um item à coleção se ainda não estiver na coleção com base no <paramref name="predicate" /> fornecido.
    /// </summary>
    /// <param name="source">A coleção</param>
    /// <param name="predicate">A condição para decidir se o item já está na coleção</param>
    /// <param name="itemFactory">Uma fábrica que retorna o item</param>
    /// <typeparam name="T">Tipo dos itens na coleção</typeparam>
    /// <returns>Retorna True se adicionado, retorna False se não for.</returns>
    public static bool AddIfNotContains<T>(this ICollection<T> source, Func<T, bool> predicate, Func<T> itemFactory)
    {
        Check.NotNull(source, "source is null");
        Check.NotNull(predicate, "predicate is null");
        Check.NotNull(itemFactory, "itemFactory is null");

        if (source.Any(predicate)) return false;
        source.Add(itemFactory());
        return true;
    }

    /// <summary>
    ///     Remove todos os itens da coleção que satisfazem o <paramref name="predicate" /> fornecido.
    /// </summary>
    /// <typeparam name="T">Tipo dos itens na coleção</typeparam>
    /// <param name="source">A coleção</param>
    /// <param name="predicate">A condição para remover os itens</param>
    /// <returns>Lista de itens removidos</returns>
    public static IList<T> RemoveAll<T>(this ICollection<T> source, Func<T, bool> predicate)
    {
        var items = source.Where(predicate).ToList();
        foreach (var item in items) source.Remove(item);
        return items;
    }

    /// <summary>
    ///     Remove todos os itens da coleção.
    /// </summary>
    /// <typeparam name="T">Tipo dos itens na coleção</typeparam>
    /// <param name="source">A coleção</param>
    /// <param name="items">Itens a serem removidos da lista</param>
    public static void RemoveAll<T>(this ICollection<T> source, IEnumerable<T> items)
    {
        foreach (var item in items) source.Remove(item);
    }
}
