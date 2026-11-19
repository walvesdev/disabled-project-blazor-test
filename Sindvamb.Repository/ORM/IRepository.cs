using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

using Sindvamb.Domain;

namespace Sindvamb.Repository.ORM
{
    public interface IRepository<TEntity, TContext, TKey>
        where TEntity : EntityBase<TKey>
        where TContext : DbContext, IBaseContext
    {
        public Guid? UserId { get; }

        public int? UserPartnerUnitId { get; set; }

        public string IpAddress { get; }
        public string Matricula { get; }

        public TContext Context { get; }

        public DbSet<TEntity> Model { get; }

        public TEntity Add(TEntity entity, bool autoSave = false);

        public Task<TEntity> AddAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public TEntity Update(TEntity entity, bool autoSave = false);

        public Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);

        public void Delete(Expression<Func<TEntity, bool>> filter = null, bool autoSave = false);

        public void Delete(TEntity entity, bool autoSave = false);

        public Task DeleteAsync(Expression<Func<TEntity, bool>> filter = null, bool autoSave = false, CancellationToken cancellationToken = default);

        public TEntity Find(params object[] id);

        public Task<TEntity> FindAsync(dynamic id, CancellationToken cancellationToken = default);

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, bool trackEntities = false);

        public Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, bool trackEntities = false, CancellationToken cancellationToken = default);
        public List<TEntity> ToList(CancellationToken cancellationToken = default);
        public Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default);

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        public Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

        public Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, bool>> sorting, CancellationToken cancellationToken = default);

        public Task EnsureCollectionLoadedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, CancellationToken cancellationToken = default) where TProperty : class;

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null);

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default);

        public bool Any(Expression<Func<TEntity, bool>> filter = null);

        public bool All(Expression<Func<TEntity, bool>> filter = null);

        public Task EnsurePropertyLoadedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression, CancellationToken cancellationToken = default) where TProperty : class;

        public Task SaveChangesAsync(bool detachAll = false, CancellationToken cancellationToken = default);

        public void SaveChanges();

        public void Restore(TEntity entity);

        public void ResetState();

        public void DetachAllEntities();

        public long Count();

        public void UseSoftDelete(bool use);

        public Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default);
    }
}
