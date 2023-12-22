using System;
using System.Threading.Tasks;

namespace WEBAPI.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitTransactionAsync();
    }
}
