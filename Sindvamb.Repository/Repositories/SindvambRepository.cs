using Sindvamb.Domain;
using Sindvamb.Repository.Contexts;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Repository.ORM;

namespace Sindvamb.Repository.Repositories;

public class SindvambRepository<TEntity, TKey> : Repository<TEntity, SindvambDbContext, TKey>, ISindvambRepository<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
    public SindvambRepository()
    {
    }

    public SindvambRepository(SindvambDbContext context) : base(context)
    {
    }
}
