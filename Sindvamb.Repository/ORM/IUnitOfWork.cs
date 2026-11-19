using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using System.Data.Common;

using Sindvamb.Domain;
using Sindvamb.Repository.Contexts;

namespace Sindvamb.Repository.ORM
{
    public interface IUnitOfWork<TContext>
         where TContext : DbContext, IBaseContext
    {
        SindvambDbContext Context { get; set; }

        DbSet<TEntity> GetDbSet<TEntity, TKey>() where TEntity : EntityBase<TKey>;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IDbContextTransaction BeginTransaction();

        void CommitTransaction();

        Task CommitTransactionAsync(CancellationToken cancellationToken);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

        IRepository<T, TContext, TKey> Repository<T, TKey>(DbTransaction? transaction = null)
            where T : EntityBase<TKey>;

        public void DetachAllEntities();
        public void RollbackTransaction();
        public Task RollbackTransactionAsync(CancellationToken cancellationToken);
    }
}
