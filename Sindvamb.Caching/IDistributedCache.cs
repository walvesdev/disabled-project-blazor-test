#region

#endregion

using JetBrains.Annotations;

using Microsoft.Extensions.Caching.Distributed;

namespace Sindvamb.Caching;

/// <summary>
///     Representa um cache distribuído de <typeparamrefname="TCacheItem" /> type.
/// </summary>
/// <typeparam name="TCacheItem">O tipo de item de cache sendo armazenado em cache.</typeparam>
public interface IDistributedCache<TCacheItem> : IDistributedCache<TCacheItem, string>
    where TCacheItem : class
{
    IDistributedCache<TCacheItem, string> InternalCache { get; }
}

/// <summary>
///     Representa um cache distribuído do tipo <typeparamref name="TCache Item" />.
///     Usa um tipo de chave de cache genérico do tipo <typeparamref name="TCache Key" />.
/// </summary>
/// <typeparam name="TCache Item">O tipo de item de cache sendo armazenado em cache.</typeparam>
/// <typeparam name="TCache Key">O tipo de chave de cache que está sendo usado.</typeparam>
public interface IDistributedCache<TCacheItem, TCacheKey>
    where TCacheItem : class
{
    /// <summary>
    ///     Obtém um item de cache com a chave fornecida. Se nenhum item de cache for encontrado para a chave fornecida,
    ///     retorna nulo.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <returns>O item de cache, ou nulo.</returns>
    TCacheItem Get(
        TCacheKey key,
        bool? hideErrors = null
    );

    /// <summary>
    ///     Obtém vários itens de cache com as chaves fornecidas.
    ///     A lista retornada contém exatamente a mesma contagem de itens especificados nas chaves fornecidas.
    ///     Um ​​item na lista de retorno não pode ser nulo, mas um item na lista tem valor nulo
    ///     se a chave relacionada não for encontrada no cache.
    /// </summary>
    /// <param name="keys">As chaves dos itens em cache a serem recuperadas do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <returns>Lista de itens de cache.</returns>
    KeyValuePair<TCacheKey, TCacheItem>[] GetMany(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null
    );

    /// <summary>
    ///     Obtém vários itens de cache com as chaves fornecidas.
    ///     A lista retornada contém exatamente a mesma contagem de itens especificados nas chaves fornecidas.
    ///     Um ​​item na lista de retorno não pode ser nulo, mas um item na lista tem valor nulo
    ///     se a chave relacionada não for encontrada no cache.
    /// </summary>
    /// <param name="keys">As chaves dos itens em cache a serem recuperadas do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// ///
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>Lista de itens de cache.</returns>
    Task<KeyValuePair<TCacheKey, TCacheItem>[]> GetManyAsync(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        CancellationToken token = default
    );

    /// <summary>
    ///     Obtém um item de cache com a chave fornecida. Se nenhum item de cache for encontrado para a chave fornecida,
    ///     retorna nulo.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O item de cache, ou nulo.</returns>
    Task<TCacheItem> GetAsync(
        [NotNull] TCacheKey key,
        bool? hideErrors = null,
        CancellationToken token = default
    );

    /// <summary>
    ///     Obtém ou adiciona um item de cache com a chave fornecida. Se nenhum item de cache for encontrado para a chave
    ///     fornecida, adicione um item de cache
    ///     fornecido por <paramref name="factory" /> delegado e retorna o item de cache fornecido.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="factory">
    ///     O delegado de fábrica é usado para fornecer o item de cache quando nenhum item de cache é
    ///     encontrado para a <paramref name="chave" /> fornecida.
    /// </param>
    /// <param name="options Factory">As ​​opções de cache para o delegado de fábrica.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <returns>O item de cache.</returns>
    TCacheItem GetOrAdd(
        TCacheKey key,
        Func<TCacheItem> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null
    );

    /// <summary>
    ///     Obtém ou adiciona um item de cache com a chave fornecida. Se nenhum item de cache for encontrado para a chave
    ///     fornecida, adicione um item de cache
    ///     fornecido por <paramref name="factory" /> delegado e retorna o item de cache fornecido.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="factory">
    ///     O delegado de fábrica é usado para fornecer o item de cache quando nenhum item de cache é
    ///     encontrado para a <paramref name="chave" /> fornecida.
    /// </param>
    /// <param name="options Factory">As ​​opções de cache para o delegado de fábrica.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O item de cache.</returns>
    Task<TCacheItem> GetOrAddAsync(
        [NotNull] TCacheKey key,
        Func<Task<TCacheItem>> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null,
        CancellationToken token = default
    );

    /// <summary>
    ///     Obtém ou adiciona vários itens de cache com as chaves fornecidas. Se algum item de cache não for encontrado para as
    ///     chaves fornecidas, adicione itens de cache
    ///     fornecido por <paramref name="factory" /> delegado e retorna os itens de cache fornecidos.
    /// </summary>
    /// <param name="keys">As chaves dos itens em cache a serem recuperadas do cache.</param>
    /// <param name="factory">
    ///     O delegado de fábrica é usado para fornecer os itens de cache quando nenhum item de cache for
    ///     encontrado para as <paramref name="keys" /> fornecidas.
    /// </param>
    /// <param name="options Factory">As ​​opções de cache para o delegado de fábrica.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <returns>Os itens do cache.</returns>
    KeyValuePair<TCacheKey, TCacheItem>[] GetOrAddMany(
        IEnumerable<TCacheKey> keys,
        Func<IEnumerable<TCacheKey>, List<KeyValuePair<TCacheKey, TCacheItem>>> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null
    );

    /// <summary>
    ///     Obtém ou adiciona vários itens de cache com as chaves fornecidas. Se algum item de cache não for encontrado para as
    ///     chaves fornecidas, adicione itens de cache
    ///     fornecido por <paramref name="factory" /> delegado e retorna os itens de cache fornecidos.
    /// </summary>
    /// <param name="keys">As chaves dos itens em cache a serem recuperadas do cache.</param>
    /// <param name="factory">
    ///     O delegado de fábrica é usado para fornecer os itens de cache quando nenhum item de cache for
    ///     encontrado para as <paramref name="keys" /> fornecidas.
    /// </param>
    /// <param name="options Factory">As ​​opções de cache para o delegado de fábrica.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>Os itens do cache.</returns>
    Task<KeyValuePair<TCacheKey, TCacheItem>[]> GetOrAddManyAsync(
        IEnumerable<TCacheKey> keys,
        Func<IEnumerable<TCacheKey>, Task<List<KeyValuePair<TCacheKey, TCacheItem>>>> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null,
        CancellationToken token = default
    );

    /// <summary>
    ///     Define o valor do item de cache para a chave fornecida.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="value">O valor do item de cache a ser definido no cache.</param>
    /// <param name="options">As opções de cache para o valor.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    void Set(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null
    );

    /// <summary>
    ///     Define o valor do item de cache para a chave fornecida.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="value">O valor do item de cache a ser definido no cache.</param>
    /// <param name="options">As opções de cache para o valor.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O <see cref="T:System.Threading.Tasks.Task" /> indicando que a operação é assíncrona.</returns>
    Task SetAsync(
        [NotNull] TCacheKey key,
        [NotNull] TCacheItem value,
        [CanBeNull] DistributedCacheEntryOptions options = null,
        bool? hideErrors = null,
        CancellationToken token = default
    );

    /// <summary>
    ///     Define vários itens de cache.
    ///     Com base na implementação, isso pode ser mais eficiente do que definir vários itens individualmente.
    /// </summary>
    /// <param name="items">Itens a serem configurados no cache</param>
    /// <param name="options">As opções de cache para o valor.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    void SetMany(
        IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null
    );

    /// <summary>
    ///     Define vários itens de cache.
    ///     Com base na implementação, isso pode ser mais eficiente do que definir vários itens individualmente.
    /// </summary>
    /// <param name="items">Itens a serem configurados no cache</param>
    /// <param name="options">As opções de cache para o valor.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O <see cref="T:System.Threading.Tasks.Task" /> indicando que a operação é assíncrona.</returns>
    Task SetManyAsync(
        IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null,
        CancellationToken token = default
    );

    /// <summary>
    ///     Atualiza o valor do cache da chave fornecida e redefine seu tempo limite de expiração deslizante.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    void Refresh(
        TCacheKey key,
        bool? hideErrors = null
    );

    /// <summary>
    ///     Atualiza o valor do cache da chave fornecida e redefine seu tempo limite de expiração deslizante.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O <see cref="T:System.Threading.Tasks.Task" /> indicando que a operação é assíncrona.</returns>
    Task RefreshAsync(
        TCacheKey key,
        bool? hideErrors = null,
        CancellationToken token = default
    );

    /// <summary>
    ///     Atualiza o valor do cache das chaves fornecidas e redefine seu tempo limite de expiração deslizante.
    ///     Com base na implementação, isso pode ser mais eficiente do que definir vários itens individualmente.
    /// </summary>
    /// <param name="keys">As chaves dos itens em cache a serem recuperadas do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    void RefreshMany(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null);

    /// <summary>
    ///     Atualiza o valor do cache das chaves fornecidas e redefine seu tempo limite de expiração deslizante.
    ///     Com base na implementação, isso pode ser mais eficiente do que definir vários itens individualmente.
    /// </summary>
    /// <param name="keys">As chaves dos itens em cache a serem recuperadas do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O <see cref="T:System.Threading.Tasks.Task" /> indicando que a operação é assíncrona.</returns>
    Task RefreshManyAsync(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        CancellationToken token = default);

    /// <summary>
    ///     Remove o item de cache para determinada chave do cache.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    void Remove(
        TCacheKey key,
        bool? hideErrors = null
    );

    /// <summary>
    ///     Remove o item de cache para determinada chave do cache.
    /// </summary>
    /// <param name="key">A chave do item em cache a ser recuperado do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O <see cref="T:System.Threading.Tasks.Task" /> indicando que a operação é assíncrona.</returns>
    Task RemoveAsync(
        TCacheKey key,
        bool? hideErrors = null,
        CancellationToken token = default
    );

    /// <summary>
    ///     Remove os itens de cache para determinadas chaves do cache.
    /// </summary>
    /// <param name="keys">As chaves dos itens em cache a serem recuperadas do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    void RemoveMany(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null
    );

    /// <summary>
    ///     Remove os itens de cache para determinadas chaves do cache.
    /// </summary>
    /// <param name="keys">As chaves dos itens em cache a serem recuperadas do cache.</param>
    /// <param name="hideErrors">Indica para lançar ou ocultar as exceções para o cache distribuído.</param>
    /// <param name="token">O <see cref="T:System.Threading.Cancellation Token" /> para a tarefa.</param>
    /// <returns>O <see cref="T:System.Threading.Tasks.Task" /> indicando que a operação é assíncrona.</returns>
    Task RemoveManyAsync(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        CancellationToken token = default
    );
}
