using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WEBAPI.Domain.Entities;

namespace WEBAPI.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TContext> : IDisposable
        where TEntity : BaseEntity, new()
        where TContext : DbContext, new()
    {
        protected readonly TContext _context;
        private DbSet<TEntity> _set;
        public GenericRepository(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = _context.Set<TEntity>();
        }


        public virtual IQueryable<TEntity> GetAllQueryable()
        {
            return _context.Set<TEntity>()
                /*.AsNoTracking()*/;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public List<TEntity> GetAll()
        {
            var entities = _context.Set<TEntity>().AsNoTracking().ToList();
            //_context.Entry(entities).State = EntityState.Detached;

            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            return entities;

        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            return entities;

        }
        public int GetCount()
        {
            return _context.Set<TEntity>().Count();
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }
        public TEntity GetById(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            _context.Entry(entity).State = EntityState.Detached;
            
            return entity;
        }

        public TEntity Add(TEntity entity, string addBy = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreateBy = addBy;
            entity.CreateDate = DateTime.Now;
            return _context.Set<TEntity>().Add(entity).Entity;
        }
        public async Task AddAsync(TEntity entity, string addBy = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreateBy = addBy;
            entity.CreateDate = DateTime.Now;
            await _context.Set<TEntity>().AddAsync(entity);
        }
        public void AddRange(List<TEntity> entities)
        {
            if (entities == null || entities.Count <= 0)
                throw new ArgumentNullException(nameof(entities));

            _context.Set<TEntity>().AddRange(entities);
        }
        public virtual async Task AddRangeAsync(List<TEntity> entities)
        {
            if (entities == null || entities.Count <= 0)
                throw new ArgumentNullException(nameof(entities));

            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public void Update(TEntity entity, string updateBy = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.LastModifiedBy = updateBy;
            entity.LastModifiedDate = DateTime.Now;
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity, bool logicalDelete = false, string deleteBy = null)
        {
            if (logicalDelete)
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity.IsDeleted = true;
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = deleteBy;
                _context.Set<TEntity>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _context.Set<TEntity>().Remove(entity);
            }
        }
        public async Task DeleteByIdAsync(int id, bool logicalDelete = false, string deleteBy = null)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            if (logicalDelete)
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity.IsDeleted = true;
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = deleteBy;
                _context.Set<TEntity>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _context.Set<TEntity>().Remove(entity);
            }
        }
        public async Task DeleteWhere(Expression<Func<TEntity, bool>> predicate = null, bool logicalDelete = false, string deleteBy = null)
        {
            if (predicate == null)
                return;

            var entities = await _context.Set<TEntity>().Where(predicate).ToListAsync();
            if (logicalDelete)
            {
                foreach (var entity in entities)
                {
                    entity.IsDeleted = true;
                    entity.LastModifiedDate = DateTime.Now;
                    entity.LastModifiedBy = deleteBy;
                    _context.Set<TEntity>().Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }
            }
            else
            {
                _context.Set<TEntity>().RemoveRange(entities);
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            // await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void SaveEntities(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            // await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}