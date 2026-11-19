#region

#endregion

using Dapper.Contrib.Extensions;

using Sindvamb.Domain;

namespace Sindvamb.Repository.Dapper;

public class DapperRepository<T, TKey> : IDapperRepository<T, TKey>
    where T : EntityBase<TKey>
{
    private List<object> ContextList { get; set; }
    public DapperDbSession Session { get; set; }

    public DapperRepository(DapperDbSession session)
    {
        Session = session;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await Session.Connection
            .GetAllAsync<T>(Session.Transaction);
    }

    public async Task<T> Find(int id)
    {
        return await Session.Connection
            .GetAsync<T>(id, Session.Transaction);
    }

    public async Task<long> Save(T model)
    {
        return await Session.Connection
            .InsertAsync(model, Session.Transaction);
    }

    public async Task<bool> Update(T model)
    {
        return await Session.Connection
            .UpdateAsync(model, Session.Transaction);
    }

    public async Task<bool> Delete(T model)
    {
        return await Session.Connection
            .DeleteAsync(model, Session.Transaction);
    }
}
