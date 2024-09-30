using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositiories
{
    public interface IGenericRepository <T> where T : BaseEntity
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);

        public Task<IReadOnlyList<T>> GetEntitiesWithSpecAsync(ISpecification<T> Spec);
        public Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec);
        public Task<int> GetCountWithSpecAsync(ISpecification<T> Spec);
        public Task AddAsync(T item);
        public void Delete(T item);
        public void Update(T item);
    }
}
