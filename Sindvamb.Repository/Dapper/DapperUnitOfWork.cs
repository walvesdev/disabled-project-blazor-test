#region

using System.Data;

using Sindvamb.Domain;

#endregion

namespace Sindvamb.Repository.Dapper;

public sealed class DapperUnitOfWork<TKey> : IDapperUnitOfWork
{
    private List<object> ContextList { get; }
    private readonly DapperDbSession _session;

    public DapperUnitOfWork(DapperDbSession session)
    {
        _session = session;
    }

    public void BeginTransaction()
    {
        _session.Transaction = _session.Connection.BeginTransaction();
    }

    public void Commit()
    {
        _session.Transaction.Commit();
        Dispose();
    }

    public void Rollback()
    {
        _session.Transaction.Rollback();
        Dispose();
    }

    public void Dispose() => _session.Transaction?.Dispose();

    public IDapperRepository<T, TKey> Repository<T, TKey>(IDbTransaction? transaction = null)
        where T : EntityBase<TKey>
    {
        if (transaction != null)
        {
            _session.Transaction = transaction;
        }


        foreach (var context in ContextList)
        {
            if (context.GetType() == typeof(DapperRepository<T, TKey>))
            {
                return context as DapperRepository<T, TKey>;
            }
        }

        var ctx = new DapperRepository<T, TKey>(_session);

        ContextList.Add(ctx);

        return ctx;
    }
}
