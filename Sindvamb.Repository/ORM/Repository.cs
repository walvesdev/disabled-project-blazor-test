using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

using Sindvamb.Domain;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Services.DependencyInjection;
using Sindvamb.Utils.Types;

namespace Sindvamb.Repository.ORM
{
    public class Repository<TEntity, TContext, TKey> : IRepository<TEntity, TContext, TKey>, IDisposable
           where TEntity : EntityBase<TKey>
           where TContext : DbContext, IBaseContext
    {
        private bool disposed = false;

        private ILazyServiceProvider LazyServiceProvider { get; set; }

        public string Matricula { get; set; }

        public Guid? UserId
        {
            get { return Context.UserId ?? null; }
        }

        public string IpAddress
        {
            get { return Context.IpAddress ?? null; }
        }

        public int? UserPartnerUnitId { get; set; }

        public Repository()
        {
            Context = AppServices.GetService<TContext>();
            InitRepository();
        }

        public Repository(TContext context)
        {
            Context = context;
            InitRepository();
        }

        private void InitRepository()
        {
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
            Context.ChangeTracker.LazyLoadingEnabled = false;
            Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
            Context.Database.SetCommandTimeout(240);

            LazyServiceProvider = AppServices.GetService<ILazyServiceProvider>();

            var httpContext = Context.HttpContext;

            if (httpContext != null && httpContext.User != null && httpContext.User.Identity?.IsAuthenticated == true)
            {
                Matricula = httpContext?.User.Claims.FirstOrDefault(c => c.Type == "Protocol")?.Value;

                var sub = httpContext?.User.Claims.FirstOrDefault(c => c.Type == "Sub")?.Value;

                if (Guid.TryParse(sub, out var userId))
                {
                    Context.UserId = userId;
                }
            }

            Context.IpAddress = AppServices.GetIpAddress();
        }

        private TEntity EntityInstance => Activator.CreateInstance(typeof(TEntity)) as TEntity;

        public TContext Context { get; private set; }

        public ICancellationTokenProvider TokenProvider => LazyServiceProvider.LazyGetService<ICancellationTokenProvider>(CancellationTokenProvider.Instance);

        public DbSet<TEntity> Model
        {
            get { return Context.Set<TEntity>(); }
        }

        /// <summary>
        /// Verifica se a entity herda de EntityBaseAudit<TKey>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool IsEntityBaseAudit<TKey>(TEntity entity)
        {
            var typeInfo = entity.GetType().GetTypeInfo();

            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(EntityBaseAudit<TKey>).GetTypeInfo().IsAssignableFrom(entity.GetType());
        }

        /// <summary>
        /// Verifica se um item foi marcado como Deletado (SoftDelete)
        /// </summary>
        /// <param name="entity"></param>
        private void CheckSoftDeleted(TEntity entity)
        {
            if ((entity as EntityBaseAudit<TKey>).IsDeleted)
            {
                throw new Exception("Item Deletado (SoftDelete).");
            }
        }

        /// <summary>
        /// Restaura um item marcado como deletado (SoftDelete)
        /// </summary>
        /// <param name="entity"></param>
        public void Restore(TEntity entity)
        {
            if (IsEntityBaseAudit<TKey>(entity))
            {
                UseSoftDelete(false);
                (entity as EntityBaseAudit<TKey>).IsDeleted = false;
                (entity as EntityBaseAudit<TKey>).DeleterId = default;
                (entity as EntityBaseAudit<TKey>).DeletedAt = null;
                Update(entity);
            }
        }

        public async Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(GetDbSet().AsQueryable());
        }

        protected DbSet<TEntity> GetDbSet()
        {
            return Context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity, bool autoSave = false)
        {
            var savedEntity = Context.Set<TEntity>().Add(entity).Entity;

            if (autoSave)
            {
                Context.SaveChanges(autoSave);
            }

            return savedEntity;
        }

        public async Task<TEntity> AddAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = (await Context.Set<TEntity>().AddAsync(entity, GetCancellationToken(cancellationToken))).Entity;

            if (autoSave)
            {
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return savedEntity;
        }

        public TEntity Update(TEntity entity, bool autoSave = false)
        {
            //DetachAllEntities();
            //Context.Attach(entity);
            var updatedEntity = Context.Set<TEntity>().Update(entity).Entity;

            if (autoSave)
            {
                Context.SaveChanges(autoSave);
            }

            return updatedEntity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            //DetachAllEntities();
            // Context.Attach(entity);

            // GetDbSet().Attach(entity);
            // var entry = Context.Entry(entity);
            // entry.State = EntityState.Modified;
            
            var updatedEntity = Context.Set<TEntity>().Update(entity).Entity;

            if (autoSave)
            {
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return updatedEntity;
        }

        public async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (entity.IsNull())
            {
                return;
            }

            await Context.Set<TEntity>().Where(t => t.Id.Equals(entity.Id))
                .ExecuteDeleteAsync();

            if (autoSave)
            {
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = Find(id);

            if (entity.IsNull())
            {
                return;
            }

            await Context.Set<TEntity>().Where(t => t.Id.Equals(entity.Id))
                .ExecuteDeleteAsync(cancellationToken: cancellationToken);

            if (autoSave)
            {
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public void Delete(Expression<Func<TEntity, bool>> filter = null, bool autoSave = false)
        {
            var list = Where(filter).ToList();

            if (list.IsNullOrEmpty())
            {
                return;
            }

            list.ForEach(item =>
            {
                Context.Set<TEntity>().Where(t => t.Id.Equals(item.Id))
                    .ExecuteDelete();
            });

            if (autoSave)
            {
                Context.SaveChanges(true);
            }
        }

        public void Delete(TEntity entity, bool autoSave = false)
        {
            if (entity.IsNull())
            {
                return;
            }

            Context.Set<TEntity>().Where(t => t.Id.Equals(entity.Id))
                .ExecuteDelete();

            if (autoSave)
            {
                Context.SaveChanges(true);
            }
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> filter = null, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var list = await Where(filter).ToListAsync(GetCancellationToken(cancellationToken));

            if (list.IsNullOrEmpty())
            {
                return;
            }

            list.ForEach(async item =>
            {
                await Context.Set<TEntity>().Where(t => t.Id.Equals(item.Id))
                    .ExecuteDeleteAsync();
            });


            if (autoSave)
            {
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        private static string GetEntityName()
        {
            var generic = typeof(TEntity);
            var instance = Activator.CreateInstance(generic);
            var typeName = instance.GetType().Name;

            return typeName;
        }

        public async Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default) => await Model.ToListAsync();

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = Model; //.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, bool trackEntities = false, CancellationToken cancellationToken = default)
        {
            if (predicate.IsNotNull())
            {
                if (trackEntities)
                {
                    var query = await Model.Where(predicate).AsTracking().ToListAsync(GetCancellationToken(cancellationToken));

                    return query.AsQueryable();
                }
                else
                {
                    var query = await Model.Where(predicate).AsNoTracking().ToListAsync(GetCancellationToken(cancellationToken));

                    return query.AsQueryable();
                }
            }
            else
            {
                if (trackEntities)
                {
                    var query = await Model.AsTracking().ToListAsync(GetCancellationToken(cancellationToken));

                    return query.AsQueryable();
                }
                else
                {
                    var query = await Model.AsNoTracking().ToListAsync(GetCancellationToken(cancellationToken));

                    return query.AsQueryable();
                }
            }
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, bool trackEntities = false)
        {
            if (predicate.IsNotNull())
            {
                if (trackEntities)
                {
                    return Model.Where(predicate).AsTracking();
                }
                else
                {
                    return Model.Where(predicate).AsNoTracking();
                }
            }
            else
            {
                if (trackEntities)
                {
                    return Model.AsTracking();
                }
                else
                {
                    return Model.AsNoTracking();
                }
            }
        }

        public List<TEntity> ToList(CancellationToken cancellationToken = default) => Model.ToList();

        public TEntity Find(params object[] id)
        {
            return Model.Find(id);
        }

        public async Task<TEntity> FindAsync(dynamic id, CancellationToken cancellationToken = default)
        {
            return await Model.FindAsync(id, GetCancellationToken(cancellationToken));
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new Exception("entities null");
            }

            Context.Set<TEntity>().UpdateRange(entities);

            if (autoSave)
            {
                await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public async Task SaveChangesAsync(bool detachAll = false, CancellationToken cancellationToken = default)
        {
            lock (this)
            {
                Task.Run(async () =>
                {
                    await Context.SaveChangesAsync(GetCancellationToken(cancellationToken));

                    if (detachAll)
                    {
                    }

                    UseSoftDelete(true);
                }, cancellationToken);

            }
        }

        public async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, bool>> sorting, CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();

            return await queryable
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task EnsureCollectionLoadedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await Context
                .Entry(entity)
                .Collection(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }

        public async Task EnsurePropertyLoadedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression, CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await Context
                .Entry(entity)
                .Reference(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }

        public bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter.IsNotNull() ? Model.Any(filter) : Model.Any();
        }

        public bool All(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter.IsNotNull() ? Model.All(filter) : false;
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter.IsNotNull() ? Model.FirstOrDefault(filter) : Model.FirstOrDefault();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            return filter.IsNotNull() ? await Model.FirstOrDefaultAsync(filter, cancellationToken: cancellationToken) : await Model.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public void SaveChanges()
        {
            lock (this)
            {
                Context.SaveChanges(true);
                UseSoftDelete(true);
            }
        }

        public void UseSoftDelete(bool use)
        {
            Context.EnableSoftDelete = use;
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }

            disposed = true;
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

        public void ResetState()
        {
            ((IResettableService)Context.ChangeTracker).ResetState();
        }

        public long Count()
        {
            return Model.Count();
        }

        protected CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
        {
            return TokenProvider.FallbackToProvider(preferredValue);
        }
    }
}
