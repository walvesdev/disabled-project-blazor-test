using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using Sindvamb.Domain;
using System.Threading;
using Sindvamb.Repository.Contexts;

namespace Sindvamb.Repository.ORM
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable //, IScopedDependency
        where TContext : DbContext, IBaseContext
    {
        private bool _disposed;

        public SindvambDbContext Context { get; set; }

        private List<object> ContextList { get; set; }

        public UnitOfWork(SindvambDbContext context, DbTransaction? transaction = null) => Init(context, transaction);

        public void Init(SindvambDbContext context, DbTransaction? transaction = null)
        {
            Context = context;
            Context.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
            Context.ChangeTracker.LazyLoadingEnabled = false;
            Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
            Context.Database.SetCommandTimeout(240);

            ContextList = new List<object>();

            if (transaction != null)
            {
                Context.Database.UseTransaction(transaction);
            }
        }

        public int SaveChanges()
        {
            return Context.SaveChanges(true);
        }

        public DbSet<TEntity> GetDbSet<TEntity, TKey>() where TEntity : EntityBase<TKey>
        {
            return Context.Set<TEntity>();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            // var saved = false;
            // while (!saved)
            // {
            //     try
            //     {
            //         // Attempt to save changes to the database
            //         await Context.SaveChangesAsync(true, cancellationToken);
            //         saved = true;
            //     }
            //     catch (DbUpdateConcurrencyException ex)
            //     {
            //         foreach (var entry in ex.Entries)
            //         {
            //             var proposedValues = entry.CurrentValues;
            //             //var databaseValues = entry?.GetDatabaseValues();
            //
            //             // foreach (var property in proposedValues.Properties)
            //             // {
            //                 // databaseValues[property] = proposedValues[property];
            //                 // var databaseValue = databaseValues[property];
            //
            //                 // TODO: decide which value should be written to database
            //                 // proposedValues[property] = <value to be saved>;
            //             // }
            //
            //             // Refresh original values to bypass next concurrency check
            //             entry.OriginalValues.SetValues(proposedValues);
            //         }
            //     }
            // }

            return await Context.SaveChangesAsync(true, cancellationToken);
            
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            await Context.Database.CommitTransactionAsync(cancellationToken);
        }

        public void CommitTransaction()
        {
            Context.Database.CommitTransaction();
        }

        public IDbContextTransaction BeginTransaction()
        {
            var level = IsolationLevel.ReadCommitted;
#if DEBUG
            level = IsolationLevel.ReadUncommitted;

#endif

            return Context.Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            var level = IsolationLevel.ReadCommitted;
#if DEBUG
            level = IsolationLevel.ReadUncommitted;
#endif
            return await Context.Database.BeginTransactionAsync(cancellationToken);
        }

        private void Dispose(bool disposing)
        {
            if (ContextList == null)
            {
                ContextList!.ForEach(ctx => ctx = null);
            }

            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void DetachAllEntities()
        {
            var entries = Context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in entries)
            {
                if (entry.Entity != null)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }

        public void RollbackTransaction()
        {
            Context.Database.RollbackTransaction();
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            await Context.Database.RollbackTransactionAsync(cancellationToken);
        }

        public IRepository<T, TContext, TKey> Repository<T, TKey>(DbTransaction? transaction = null)
            where T : EntityBase<TKey>
        {
            if (transaction != null)
            {
                Context.Database.UseTransaction(transaction);
            }

            var ctx = new Repository<T, TContext, TKey>(Context as TContext);

            ContextList.Add(ctx);

            return ctx;
        }
    }
}