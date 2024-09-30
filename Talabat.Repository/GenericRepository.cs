using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositiories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly TalabatContext _dbContext;
        public GenericRepository(TalabatContext dbContext) 
        {
            _dbContext = dbContext;
        }
        #region Before Specification Design Pattern
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        #endregion
        public async Task<IReadOnlyList<T>> GetEntitiesWithSpecAsync(ISpecification<T> Spec)
        {
            return await ApplySpec(Spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec)
        {
            return await ApplySpec(Spec).FirstOrDefaultAsync();
        }
        public Task<int> GetCountWithSpecAsync(ISpecification<T> Spec)
        {
            return ApplySpec(Spec).CountAsync();
        }
        private IQueryable<T> ApplySpec(ISpecification<T> Spec)
        {
           return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public async Task AddAsync(T item)
        
         =>   await _dbContext.Set<T>().AddAsync(item);

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
        }
    }
}
