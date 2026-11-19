using Sindvamb.Domain;
using Sindvamb.Repository.Contexts;
using Sindvamb.Repository.ORM;

namespace Sindvamb.Repository.Interfaces;

public interface ISindvambRepository<TEntity, TKey> : IRepository<TEntity, SindvambDbContext, TKey>
    where TEntity : EntityBase<TKey>
{

}
