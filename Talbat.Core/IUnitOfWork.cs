using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositiories;

namespace Talabat.Core
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        Task<int> CompleteAsync();
        IGenericRepository<T> Repository<T>() where T:BaseEntity;
    }
}
