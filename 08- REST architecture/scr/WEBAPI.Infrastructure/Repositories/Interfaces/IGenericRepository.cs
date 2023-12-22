using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace WEBAPI.Infrastructure.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAllQueryable();

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        int GetCount();
        Task<int> GetCountAsync();
        TEntity GetById(int id);
        Task<TEntity> GetByIdAsync(int id);

        TEntity Add(TEntity entity, string addBy = null);
        Task AddAsync(TEntity entity, string addBy = null);
        void AddRange(List<TEntity> entities);
        Task AddRangeAsync(List<TEntity> entities);

        void Update(TEntity entity, string updateBy = null);
        void Delete(TEntity entity, bool logicalDelete = false, string deleteBy = null);
        Task DeleteByIdAsync(int id, bool logicalDelete = false, string deleteBy = null);
        Task DeleteWhere(Expression<Func<TEntity, bool>> predicate = null, bool logicalDelete = false, string deleteBy = null);
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
        void SaveEntities(CancellationToken cancellationToken = default(CancellationToken));
    }
}

