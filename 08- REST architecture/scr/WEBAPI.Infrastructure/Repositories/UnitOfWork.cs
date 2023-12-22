using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WEBAPI.Infrastructure.Repositories.Interfaces;

namespace WEBAPI.Infrastructure.Repositories
{
    public class UnitOfWork  : IUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;
        public UnitOfWork(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task CommitTransactionAsync()
        {
            var executionStrategy = _applicationContext.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _applicationContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _applicationContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            });
        }

        public void Dispose()
        {
            _applicationContext.Dispose();
        }
    }
}
