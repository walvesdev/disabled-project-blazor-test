#region

#endregion

using Sindvamb.Domain;

namespace Sindvamb.Repository.Dapper;

public interface IDapperRepository<T, TKey>
    where T : EntityBase<TKey>
{
    DapperDbSession Session { get; set; }

    Task<IEnumerable<T>> GetAll();

    Task<T> Find(int id);

    Task<long> Save(T model);

    Task<bool> Update(T model);

    Task<bool> Delete(T model);
}
