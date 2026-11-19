#region

#endregion

using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Types;

namespace Sindvamb.Caching;

/// <summary>
///     Representa um cache distribuído de <typeparamrefname="TCacheItem" /> type.
/// </summary>
/// <typeparam name="TCacheItem">O tipo de item de cache sendo armazenado em cache.</typeparam>
public class DistributedCache<TCacheItem> : IDistributedCache<TCacheItem>
    where TCacheItem : class
{
    public IDistributedCache<TCacheItem, string> InternalCache { get; }
    public ILogger Logger { get; set; }

    public DistributedCache(IDistributedCache<TCacheItem, string> internalCache, ILogger logger)
    {
        InternalCache = internalCache;
        Logger = logger;
    }

    public TCacheItem Get(string key, bool? hideErrors = null)
    {
        return InternalCache.Get(key, hideErrors);
    }

    public KeyValuePair<string, TCacheItem>[] GetMany(IEnumerable<string> keys, bool? hideErrors = null)
    {
        return InternalCache.GetMany(keys, hideErrors);
    }

    public Task<KeyValuePair<string, TCacheItem>[]> GetManyAsync(IEnumerable<string> keys, bool? hideErrors = null, CancellationToken token = default)
    {
        return InternalCache.GetManyAsync(keys, hideErrors, token);
    }

    public Task<TCacheItem> GetAsync(string key, bool? hideErrors = null, CancellationToken token = default)
    {
        return InternalCache.GetAsync(key, hideErrors, token);
    }

    public TCacheItem GetOrAdd(string key, Func<TCacheItem> factory, Func<DistributedCacheEntryOptions> optionsFactory = null, bool? hideErrors = null)
    {
        return InternalCache.GetOrAdd(key, factory, optionsFactory, hideErrors);
    }

    public Task<TCacheItem> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory, Func<DistributedCacheEntryOptions> optionsFactory = null, bool? hideErrors = null, CancellationToken token = default)
    {
        return InternalCache.GetOrAddAsync(key, factory, optionsFactory, hideErrors, token);
    }

    public KeyValuePair<string, TCacheItem>[] GetOrAddMany(IEnumerable<string> keys, Func<IEnumerable<string>, List<KeyValuePair<string, TCacheItem>>> factory, Func<DistributedCacheEntryOptions> optionsFactory = null, bool? hideErrors = null)
    {
        return InternalCache.GetOrAddMany(keys, factory, optionsFactory, hideErrors);
    }

    public Task<KeyValuePair<string, TCacheItem>[]> GetOrAddManyAsync(IEnumerable<string> keys, Func<IEnumerable<string>, Task<List<KeyValuePair<string, TCacheItem>>>> factory, Func<DistributedCacheEntryOptions> optionsFactory = null, bool? hideErrors = null, CancellationToken token = default)
    {
        return InternalCache.GetOrAddManyAsync(keys, factory, optionsFactory, hideErrors, token);
    }

    public void Set(string key, TCacheItem value, DistributedCacheEntryOptions options = null, bool? hideErrors = null)
    {
        InternalCache.Set(key, value, options, hideErrors);
    }

    public Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions options = null, bool? hideErrors = null, CancellationToken token = default)
    {
        return InternalCache.SetAsync(key, value, options, hideErrors, token);
    }

    public void SetMany(IEnumerable<KeyValuePair<string, TCacheItem>> items, DistributedCacheEntryOptions options = null, bool? hideErrors = null)
    {
        InternalCache.SetMany(items, options, hideErrors);
    }

    public Task SetManyAsync(IEnumerable<KeyValuePair<string, TCacheItem>> items, DistributedCacheEntryOptions options = null, bool? hideErrors = null, CancellationToken token = default)
    {
        return InternalCache.SetManyAsync(items, options, hideErrors, token);
    }

    public void Refresh(string key, bool? hideErrors = null)
    {
        InternalCache.Refresh(key, hideErrors);
    }

    public Task RefreshAsync(string key, bool? hideErrors = null, CancellationToken token = default)
    {
        return InternalCache.RefreshAsync(key, hideErrors, token);
    }

    public void RefreshMany(IEnumerable<string> keys, bool? hideErrors = null)
    {
        InternalCache.RefreshMany(keys, hideErrors);
    }

    public Task RefreshManyAsync(IEnumerable<string> keys, bool? hideErrors = null, CancellationToken token = default)
    {
        return InternalCache.RefreshManyAsync(keys, hideErrors, token);
    }

    public void Remove(string key, bool? hideErrors = null)
    {
        InternalCache.Remove(key, hideErrors);
    }

    public Task RemoveAsync(string key, bool? hideErrors = null, CancellationToken token = default)
    {
        return InternalCache.RemoveAsync(key, hideErrors, token);
    }

    public void RemoveMany(IEnumerable<string> keys, bool? hideErrors = null)
    {
        InternalCache.RemoveMany(keys, hideErrors);
    }

    public Task RemoveManyAsync(IEnumerable<string> keys, bool? hideErrors = null,
        CancellationToken token = default)
    {
        return InternalCache.RemoveManyAsync(keys, hideErrors, token);
    }
}

/// <summary>
///     Representa um cache distribuído de<typeparamref name="TCacheItem" /> tipo.
///     Usa um tipo de chave de cache genérico de <typeparamref name="TCacheKey" /> tipo.
/// </summary>
/// <typeparam name="TCacheItem">O tipo de item de cache sendo armazenado em cache.</typeparam>
/// <typeparam name="TCacheKey">O tipo de chave de cache que está sendo usado.</typeparam>
public class DistributedCache<TCacheItem, TCacheKey> : IDistributedCache<TCacheItem, TCacheKey>
    where TCacheItem : class
{
    public ILogger<DistributedCache<TCacheItem, TCacheKey>> Logger { get; set; }

    protected string CacheName { get; set; }

    protected IDistributedCache Cache { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    protected IDistributedCacheSerializer Serializer { get; }

    protected IDistributedCacheKeyNormalizer KeyNormalizer { get; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected SemaphoreSlim SyncSemaphore { get; }

    protected DistributedCacheEntryOptions DefaultCacheOptions;

    private readonly DistributedCacheOptions _distributedCacheOption;

    public DistributedCache(
        IOptions<DistributedCacheOptions> distributedCacheOption,
        IDistributedCache cache,
        ICancellationTokenProvider cancellationTokenProvider,
        IDistributedCacheSerializer serializer,
        IDistributedCacheKeyNormalizer keyNormalizer,
        IServiceScopeFactory serviceScopeFactory)
    {
        _distributedCacheOption = distributedCacheOption.Value;
        Cache = cache;
        CancellationTokenProvider = cancellationTokenProvider;
        Logger = NullLogger<DistributedCache<TCacheItem, TCacheKey>>.Instance;
        Serializer = serializer;
        KeyNormalizer = keyNormalizer;
        ServiceScopeFactory = serviceScopeFactory;

        SyncSemaphore = new SemaphoreSlim(1, 1);

        SetDefaultOptions();
    }

    protected virtual string NormalizeKey(TCacheKey key)
    {
        return KeyNormalizer.NormalizeKey(
            new DistributedCacheKeyNormalizeArgs(
                key.ToString(),
                CacheName
            )
        );
    }


    protected virtual DistributedCacheEntryOptions GetDefaultCacheEntryOptions()
    {
        foreach (var configure in _distributedCacheOption.CacheConfigurators)
        {
            var options = configure.Invoke(CacheName);
            if (options != null)
            {
                return options;
            }
        }

        return _distributedCacheOption.GlobalCacheEntryOptions;
    }

    protected virtual void SetDefaultOptions()
    {
        CacheName = CacheNameAttribute.GetCacheName(typeof(TCacheItem));

        //Configurar opções de entrada de cache padrão
        DefaultCacheOptions = GetDefaultCacheEntryOptions();
    }

    /// <summary>
    ///     Obtém um item de cache com a chave fornecida. Se nenhum item de cache for encontrado para a chave fornecida,
    ///     retorna null.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">
    ///     Isso armazenará o cache na unidade de trabalho atual até o final da unidade de trabalho atual
    ///     não afeta realmente o cache.
    /// </param>
    /// <returns>O item de cache ou nulo.</returns>
    public virtual TCacheItem Get(
        TCacheKey key,
        bool? hideErrors = null
    )
    {
        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

        byte[] cachedBytes;

        try
        {
            cachedBytes = Cache.Get(NormalizeKey(key));
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                HandleException(ex);
                return null;
            }

            throw;
        }

        return ToCacheItem(cachedBytes);
    }

    public virtual KeyValuePair<TCacheKey, TCacheItem>[] GetMany(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null
    )
    {
        var keyArray = keys.ToArray();

        var cacheSupportsMultipleItems = Cache as ICacheSupportsMultipleItems;
        if (cacheSupportsMultipleItems == null)
        {
            return GetManyFallback(
                keyArray,
                hideErrors
            );
        }

        var notCachedKeys = new List<TCacheKey>();
        var cachedValues = new List<KeyValuePair<TCacheKey, TCacheItem>>();

        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;
        byte[][] cachedBytes;

        var readKeys = notCachedKeys.Any() ? notCachedKeys.ToArray() : keyArray;
        try
        {
            cachedBytes = cacheSupportsMultipleItems.GetMany(readKeys.Select(NormalizeKey));
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                HandleException(ex);
                return ToCacheItemsWithDefaultValues(keyArray);
            }

            throw;
        }

        return cachedValues.Concat(ToCacheItems(cachedBytes, readKeys)).ToArray();
    }

    protected virtual KeyValuePair<TCacheKey, TCacheItem>[] GetManyFallback(
        TCacheKey[] keys,
        bool? hideErrors = null
    )
    {
        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

        try
        {
            return keys
                .Select(key => new KeyValuePair<TCacheKey, TCacheItem>(
                        key,
                        Get(key, false)
                    )
                ).ToArray();
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                HandleException(ex);
                return ToCacheItemsWithDefaultValues(keys);
            }

            throw;
        }
    }

    public virtual async Task<KeyValuePair<TCacheKey, TCacheItem>[]> GetManyAsync(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        var keyArray = keys.ToArray();

        var cacheSupportsMultipleItems = Cache as ICacheSupportsMultipleItems;
        if (cacheSupportsMultipleItems == null)
        {
            return await GetManyFallbackAsync(
                keyArray,
                hideErrors,
                token
            );
        }

        var notCachedKeys = new List<TCacheKey>();
        var cachedValues = new List<KeyValuePair<TCacheKey, TCacheItem>>();

        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;
        byte[][] cachedBytes;

        var readKeys = notCachedKeys.Any() ? notCachedKeys.ToArray() : keyArray;

        try
        {
            cachedBytes = await cacheSupportsMultipleItems.GetManyAsync(
                readKeys.Select(NormalizeKey),
                CancellationTokenProvider.FallbackToProvider(token)
            );
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                await HandleExceptionAsync(ex);
                return ToCacheItemsWithDefaultValues(keyArray);
            }

            throw;
        }

        return cachedValues.Concat(ToCacheItems(cachedBytes, readKeys)).ToArray();
    }

    protected virtual async Task<KeyValuePair<TCacheKey, TCacheItem>[]> GetManyFallbackAsync(
        TCacheKey[] keys,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

        try
        {
            var result = new List<KeyValuePair<TCacheKey, TCacheItem>>();

            foreach (var key in keys)
            {
                result.Add(new KeyValuePair<TCacheKey, TCacheItem>(
                    key,
                    await GetAsync(key, false, token: token))
                );
            }

            return result.ToArray();
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                await HandleExceptionAsync(ex);
                return ToCacheItemsWithDefaultValues(keys);
            }

            throw;
        }
    }

    /// <summary>
    ///     Obtém um item de cache com a chave fornecida. Se nenhum item de cache for encontrado para a chave fornecida,
    ///     retorna null.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indica lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="considerUow">
    ///     Isso armazenará o cache na unidade de trabalho atual até o final da unidade de trabalho atual
    ///     não afeta realmente o cache.
    /// </param>
    /// <param name="token">O <see cref="T:System.Threading.CancellationToken" /> para a tarefa.</param>
    /// <returns>O item de cache ou nulo.</returns>
    public virtual async Task<TCacheItem> GetAsync(
        TCacheKey key,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

        byte[] cachedBytes;

        try
        {
            cachedBytes = await Cache.GetAsync(
                NormalizeKey(key),
                CancellationTokenProvider.FallbackToProvider(token)
            );
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                await HandleExceptionAsync(ex);
                return null;
            }

            throw;
        }

        if (cachedBytes == null)
        {
            return null;
        }

        return Serializer.Deserialize<TCacheItem>(cachedBytes);
    }

    /// <summary>
    ///     Obtém ou adiciona um item de cache com a chave fornecida. Se nenhum item de cache for encontrado para a chave
    ///     fornecida, adicione um item de cache
    ///     fornecido por <paramref name="factory" /> delegate e retorna o item de cache fornecido.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="factory">
    ///     O delegado de fábrica é usado para fornecer o item de cache quando nenhum item de cache é
    ///     encontrado para o dado<paramref name="key" />.
    /// </param>
    /// <param name="optionsFactory">As opções de cache para o delegado de fábrica.</param>
    /// <param name="hideErrors">Indica lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="considerUow">
    ///     Isso armazenará o cache na unidade de trabalho atual até o final da unidade de trabalho atual
    ///     não afeta realmente o cache.
    /// </param>
    /// <returns>O item de cache.</returns>
    public virtual TCacheItem GetOrAdd(
        TCacheKey key,
        Func<TCacheItem> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null
    )
    {
        var value = Get(key, hideErrors);
        if (value != null)
        {
            return value;
        }

        using (SyncSemaphore.Lock())
        {
            value = Get(key, hideErrors);
            if (value != null)
            {
                return value;
            }

            value = factory();

            Set(key, value, optionsFactory?.Invoke(), hideErrors);
        }

        return value;
    }

    /// <summary>
    ///     Obtém ou adiciona um item de cache com a chave fornecida. Se nenhum item de cache for encontrado para a chave
    ///     fornecida, adicione um item de cache
    ///     fornecido por <paramref name="factory" /> delegate e retorna o item de cache fornecido.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="factory">
    ///     O delegado de fábrica é usado para fornecer o item de cache quando nenhum item de cache é
    ///     encontrado para o dado<paramref name="key" />.
    /// </param>
    /// <param name="optionsFactory">As opções de cache para o delegado de fábrica.</param>
    /// <param name="hideErrors">Indica lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="considerUow">
    ///     Isso armazenará o cache na unidade de trabalho atual até o final da unidade de trabalho atual
    ///     não afeta realmente o cache.
    /// </param>
    /// <param name="token">o <see cref="T:System.Threading.CancellationToken" /> para a tarefa.</param>
    /// <returns>O item de cache.</returns>
    public virtual async Task<TCacheItem> GetOrAddAsync(
        TCacheKey key,
        Func<Task<TCacheItem>> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        token = CancellationTokenProvider.FallbackToProvider(token);
        var value = await GetAsync(key, hideErrors, token);
        if (value != null)
        {
            return value;
        }

        using (await SyncSemaphore.LockAsync(token))
        {
            value = await GetAsync(key, hideErrors, token);
            if (value != null)
            {
                return value;
            }

            value = await factory();

            await SetAsync(key, value, optionsFactory?.Invoke(), hideErrors, token);
        }

        return value;
    }

    public KeyValuePair<TCacheKey, TCacheItem>[] GetOrAddMany(
        IEnumerable<TCacheKey> keys,
        Func<IEnumerable<TCacheKey>, List<KeyValuePair<TCacheKey, TCacheItem>>> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null
    )
    {
        KeyValuePair<TCacheKey, TCacheItem>[] result;
        var keyArray = keys.ToArray();

        var cacheSupportsMultipleItems = Cache as ICacheSupportsMultipleItems;
        if (cacheSupportsMultipleItems == null)
        {
            result = GetManyFallback(
                keyArray,
                hideErrors
            );
        }
        else
        {
            var notCachedKeys = new List<TCacheKey>();
            var cachedValues = new List<KeyValuePair<TCacheKey, TCacheItem>>();

            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;
            byte[][] cachedBytes;

            var readKeys = notCachedKeys.Any() ? notCachedKeys.ToArray() : keyArray;
            try
            {
                cachedBytes = cacheSupportsMultipleItems.GetMany(readKeys.Select(NormalizeKey));
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    HandleException(ex);
                    return ToCacheItemsWithDefaultValues(keyArray);
                }

                throw;
            }

            result = cachedValues.Concat(ToCacheItems(cachedBytes, readKeys)).ToArray();
        }

        if (result.All(x => x.Value != null))
        {
            return result;
        }

        var missingKeys = new List<TCacheKey>();
        var missingValuesIndex = new List<int>();
        for (var i = 0; i < keyArray.Length; i++)
        {
            if (result[i].Value != null)
            {
                continue;
            }

            missingKeys.Add(keyArray[i]);
            missingValuesIndex.Add(i);
        }

        var missingValues = factory.Invoke(missingKeys).ToArray();
        var valueQueue = new Queue<KeyValuePair<TCacheKey, TCacheItem>>(missingValues);

        SetMany(missingValues, optionsFactory?.Invoke(), hideErrors);

        foreach (var index in missingValuesIndex)
        {
            result[index] = valueQueue.Dequeue();
        }

        return result;
    }


    public async Task<KeyValuePair<TCacheKey, TCacheItem>[]> GetOrAddManyAsync(
        IEnumerable<TCacheKey> keys,
        Func<IEnumerable<TCacheKey>, Task<List<KeyValuePair<TCacheKey, TCacheItem>>>> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        KeyValuePair<TCacheKey, TCacheItem>[] result;
        var keyArray = keys.ToArray();

        var cacheSupportsMultipleItems = Cache as ICacheSupportsMultipleItems;
        if (cacheSupportsMultipleItems == null)
        {
            result = await GetManyFallbackAsync(
                keyArray,
                hideErrors,
                token);
        }
        else
        {
            var notCachedKeys = new List<TCacheKey>();
            var cachedValues = new List<KeyValuePair<TCacheKey, TCacheItem>>();

            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;
            byte[][] cachedBytes;

            var readKeys = notCachedKeys.Any() ? notCachedKeys.ToArray() : keyArray;
            try
            {
                cachedBytes = await cacheSupportsMultipleItems.GetManyAsync(readKeys.Select(NormalizeKey), token);
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    await HandleExceptionAsync(ex);
                    return ToCacheItemsWithDefaultValues(keyArray);
                }

                throw;
            }

            result = cachedValues.Concat(ToCacheItems(cachedBytes, readKeys)).ToArray();
        }

        if (result.All(x => x.Value != null))
        {
            return result;
        }

        var missingKeys = new List<TCacheKey>();
        var missingValuesIndex = new List<int>();
        for (var i = 0; i < keyArray.Length; i++)
        {
            if (result[i].Value != null)
            {
                continue;
            }

            missingKeys.Add(keyArray[i]);
            missingValuesIndex.Add(i);
        }

        var missingValues = (await factory.Invoke(missingKeys)).ToArray();
        var valueQueue = new Queue<KeyValuePair<TCacheKey, TCacheItem>>(missingValues);

        await SetManyAsync(missingValues, optionsFactory?.Invoke(), hideErrors, token);

        foreach (var index in missingValuesIndex)
        {
            result[index] = valueQueue.Dequeue();
        }

        return result;
    }

    /// <summary>
    ///     Define o valor do item de cache para a chave fornecida.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="value">O valor do item de cache a ser definido no cache.</param>
    /// <param name="options">As opções de cache para o valor.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    public virtual void Set(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null)
    {
        void SetRealCache()
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                Cache.Set(
                    NormalizeKey(key),
                    Serializer.Serialize(value),
                    options ?? DefaultCacheOptions
                );
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    HandleException(ex);
                    return;
                }

                throw;
            }
        }


        SetRealCache();
    }

    /// <summary>
    ///     Define o valor do item de cache para a chave fornecida.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="value">O valor do item de cache a ser definido no cache.</param>
    /// <param name="options">As opções de cache para o valor.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O <see cref="T:System.Threading.Tasks.Task" /> indicando que a operação é assíncrona.</returns>
    public virtual async Task SetAsync(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        async Task SetRealCache()
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                await Cache.SetAsync(
                    NormalizeKey(key),
                    Serializer.Serialize(value),
                    options ?? DefaultCacheOptions,
                    CancellationTokenProvider.FallbackToProvider(token)
                );
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    await HandleExceptionAsync(ex);
                    return;
                }

                throw;
            }
        }


        await SetRealCache();
    }

    public void SetMany(
        IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null
    )
    {
        var itemsArray = items.ToArray();

        var cacheSupportsMultipleItems = Cache as ICacheSupportsMultipleItems;
        if (cacheSupportsMultipleItems == null)
        {
            SetManyFallback(
                itemsArray,
                options,
                hideErrors
            );

            return;
        }

        void SetRealCache()
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                cacheSupportsMultipleItems.SetMany(
                    ToRawCacheItems(itemsArray),
                    options ?? DefaultCacheOptions
                );
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    HandleException(ex);
                    return;
                }

                throw;
            }
        }


        SetRealCache();
    }

    protected virtual void SetManyFallback(
        KeyValuePair<TCacheKey, TCacheItem>[] items,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null
    )
    {
        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

        try
        {
            foreach (var item in items)
            {
                Set(
                    item.Key,
                    item.Value,
                    options,
                    false
                );
            }
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                HandleException(ex);
                return;
            }

            throw;
        }
    }

    public virtual async Task SetManyAsync(
        IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        var itemsArray = items.ToArray();

        var cacheSupportsMultipleItems = Cache as ICacheSupportsMultipleItems;
        if (cacheSupportsMultipleItems == null)
        {
            await SetManyFallbackAsync(
                itemsArray,
                options,
                hideErrors,
                token
            );

            return;
        }

        async Task SetRealCache()
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                await cacheSupportsMultipleItems.SetManyAsync(
                    ToRawCacheItems(itemsArray),
                    options ?? DefaultCacheOptions,
                    CancellationTokenProvider.FallbackToProvider(token)
                );
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    await HandleExceptionAsync(ex);
                    return;
                }

                throw;
            }
        }

        await SetRealCache();
    }

    protected virtual async Task SetManyFallbackAsync(
        KeyValuePair<TCacheKey, TCacheItem>[] items,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

        try
        {
            foreach (var item in items)
            {
                await SetAsync(
                    item.Key,
                    item.Value,
                    options,
                    false,
                    token: token
                );
            }
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                await HandleExceptionAsync(ex);
                return;
            }

            throw;
        }
    }

    /// <summary>
    ///     Atualiza o valor do cache da chave fornecida e redefine seu tempo limite de expiração deslizante.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    public virtual void Refresh(
        TCacheKey key,
        bool? hideErrors = null)
    {
        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

        try
        {
            Cache.Refresh(NormalizeKey(key));
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                HandleException(ex);
                return;
            }

            throw;
        }
    }

    /// <summary>
    ///     Atualiza o valor do cache da chave fornecida e redefine seu tempo limite de expiração deslizante.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O <see cref="T:System.Threading.Tasks.Task" /> indicando que a operação é assíncrona.</returns>
    public virtual async Task RefreshAsync(
        TCacheKey key,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

        try
        {
            await Cache.RefreshAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                await HandleExceptionAsync(ex);
                return;
            }

            throw;
        }
    }

    public virtual void RefreshMany(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null)
    {
        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

        try
        {
            if (Cache is ICacheSupportsMultipleItems cacheSupportsMultipleItems)
            {
                cacheSupportsMultipleItems.RefreshMany(keys.Select(NormalizeKey));
            }
            else
            {
                foreach (var key in keys)
                {
                    Cache.Refresh(NormalizeKey(key));
                }
            }
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                HandleException(ex);
                return;
            }

            throw;
        }
    }

    public virtual async Task RefreshManyAsync(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

        try
        {
            if (Cache is ICacheSupportsMultipleItems cacheSupportsMultipleItems)
            {
                await cacheSupportsMultipleItems.RefreshManyAsync(keys.Select(NormalizeKey), token);
            }
            else
            {
                foreach (var key in keys)
                {
                    await Cache.RefreshAsync(NormalizeKey(key), token);
                }
            }
        }
        catch (Exception ex)
        {
            if (hideErrors == true)
            {
                await HandleExceptionAsync(ex);
                return;
            }

            throw;
        }
    }

    /// <summary>
    ///     Remove o item de cache para determinada chave do cache.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// )
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    public virtual void Remove(
        TCacheKey key,
        bool? hideErrors = null
    )
    {
        void RemoveRealCache()
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                Cache.Remove(NormalizeKey(key));
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    HandleException(ex);
                    return;
                }

                throw;
            }
        }

        RemoveRealCache();
    }

    /// <summary>
    ///     Remove o item de cache para determinada chave do cache.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// )
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O <see cref="T:System.Threading.Tasks.Task" /> indicando que a operação é assíncrona.</returns>
    public virtual async Task RemoveAsync(
        TCacheKey key,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        async Task RemoveRealCache()
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                await Cache.RemoveAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    await HandleExceptionAsync(ex);
                    return;
                }

                throw;
            }
        }

        await RemoveRealCache();
    }

    public void RemoveMany(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null
    )
    {
        var keyArray = keys.ToArray();

        if (Cache is ICacheSupportsMultipleItems cacheSupportsMultipleItems)
        {
            void RemoveRealCache()
            {
                hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

                try
                {
                    cacheSupportsMultipleItems.RemoveMany(
                        keyArray.Select(NormalizeKey)
                    );
                }
                catch (Exception ex)
                {
                    if (hideErrors == true)
                    {
                        HandleException(ex);
                        return;
                    }

                    throw;
                }
            }


            RemoveRealCache();
        }
        else
        {
            foreach (var key in keyArray)
            {
                Remove(key, hideErrors);
            }
        }
    }

    public async Task RemoveManyAsync(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        var keyArray = keys.ToArray();

        if (Cache is ICacheSupportsMultipleItems cacheSupportsMultipleItems)
        {
            async Task RemoveRealCache()
            {
                hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

                try
                {
                    await cacheSupportsMultipleItems.RemoveManyAsync(
                        keyArray.Select(NormalizeKey), token);
                }
                catch (Exception ex)
                {
                    if (hideErrors == true)
                    {
                        await HandleExceptionAsync(ex);
                        return;
                    }

                    throw;
                }
            }

            await RemoveRealCache();
        }
        else
        {
            foreach (var key in keyArray)
            {
                await RemoveAsync(key, hideErrors, token);
            }
        }
    }

    protected virtual void HandleException(Exception ex)
    {
        _ = HandleExceptionAsync(ex);
    }

    protected virtual async Task HandleExceptionAsync(Exception ex)
    {
        Logger.LogError(ex.Serialize());

        throw ex;
    }

    protected virtual IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> ToCacheItems(byte[][] itemBytes, TCacheKey[] itemKeys)
    {
        if (itemBytes.Length != itemKeys.Length)
        {
            throw new Exception("count of the item bytes should be same with the count of the given keys");
        }

        var result = new List<KeyValuePair<TCacheKey, TCacheItem>>();

        for (int i = 0; i < itemKeys.Length; i++)
        {
            result.Add(
                new KeyValuePair<TCacheKey, TCacheItem>(
                    itemKeys[i],
                    ToCacheItem(itemBytes[i])
                )
            );
        }

        return result.ToArray();
    }

    [CanBeNull]
    protected virtual TCacheItem ToCacheItem([CanBeNull] byte[] bytes)
    {
        if (bytes == null)
        {
            return null;
        }

        return Serializer.Deserialize<TCacheItem>(bytes);
    }


    protected virtual KeyValuePair<string, byte[]>[] ToRawCacheItems(KeyValuePair<TCacheKey, TCacheItem>[] items)
    {
        return items
            .Select(i => new KeyValuePair<string, byte[]>(
                    NormalizeKey(i.Key),
                    Serializer.Serialize(i.Value)
                )
            ).ToArray();
    }

    private static KeyValuePair<TCacheKey, TCacheItem>[] ToCacheItemsWithDefaultValues(TCacheKey[] keys)
    {
        return keys
            .Select(key => new KeyValuePair<TCacheKey, TCacheItem>(key, default))
            .ToArray();
    }
}
