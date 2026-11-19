
using System.ComponentModel.DataAnnotations;

using Sindvamb.Utils.Extension;


namespace Sindvamb.Utils.Extension;

public static class KeyValuePairExtension
{
    public static TModel GetValue<TKey, TModel>(this KeyValuePair<TKey, TModel> keyValuePair)
    {
        return keyValuePair.Value;
    }

    public static TKey GetKey<TKey, TModel>(this KeyValuePair<TKey, TModel> keyValuePair)
    {
        return keyValuePair.Key;
    }

    public static List<TModel> GetValues<TKey, TModel>(this KeyValuePair<TKey, TModel>[] keyValuePair)
    {
        return keyValuePair.Select(v => v.Value).ToList();
    }

    public static KeyValuePair<TKey, TModel> SetModelValue<TKey, TModel>(this KeyValuePair<TKey, TModel> keyValuePair, TModel value)
    {
        var newEntry = new KeyValuePair<TKey, TModel>(keyValuePair.Key, value);
        return newEntry;
    }

    public static KeyValuePair<TKey, TModel> SetModelKey<TKey, TModel>(this KeyValuePair<TKey, TModel> keyValuePair, TKey value)
    {
        var newEntry = new KeyValuePair<TKey, TModel>(value, keyValuePair.Value);
        return newEntry;
    }

    public static KeyValuePair<TKey, TModel> SetValue<TKey, TModel>(this KeyValuePair<TKey, TModel> keyValuePair, TKey key, TModel value)
    {
        var newEntry = new KeyValuePair<TKey, TModel>(key, value);
        return newEntry;
    }

    public static KeyValuePair<int, TModel>[] ToKeyValuePairs<TModel>(this IEnumerable<TModel> list)
    {
        var newEntry = new List<KeyValuePair<int, TModel>>();

        if (list.IsNullOrEmpty())
        {
            return default;
        }

        var i = 1;

        foreach (var item in list)
        {
            newEntry.Add(new KeyValuePair<int, TModel>(i, item));
            i++;
        }

        return newEntry.ToArray();
    }

    public static KeyValuePair<dynamic, TModel>[] ToKeyValuePairs<TKey, TModel>(this IEnumerable<TModel> list) where TModel : new()
    {
        var newEntry = new List<KeyValuePair<dynamic, TModel>>();

        if (list.IsNullOrEmpty())
        {
            return newEntry.ToArray();
        }

        foreach (var item in list)
        {
            var property = typeof(TModel).GetProperties().Where(p => p.IsDefined(typeof(KeyAttribute), false)).FirstOrDefault();

            if (property.IsNotNull())
            {
                var propValue = property.GetValue(item, null);

                newEntry.Add(new KeyValuePair<dynamic, TModel>(propValue, item));
            }
        }

        return newEntry.ToArray();
    }
}
