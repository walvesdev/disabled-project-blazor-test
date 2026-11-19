#region

using System.Data;

using Sindvamb.Domain;

#endregion

namespace Sindvamb.Repository.Dapper;

public interface IDapperUnitOfWork : IDisposable

{
    IDapperRepository<T, TKey> Repository<T, TKey>(IDbTransaction? transaction = null)
        where T : EntityBase<TKey>;

    void BeginTransaction();

    void Commit();

    void Rollback();
}
