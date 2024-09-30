using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositiories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TalabatContext _dbContext;
        private Hashtable Repositories;
        public UnitOfWork(TalabatContext dbContext)
        {
            _dbContext = dbContext;
            Repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        
        => await _dbContext.SaveChangesAsync();
        

        public async ValueTask DisposeAsync()
        
        =>  await _dbContext.DisposeAsync();
        

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;
            if (!Repositories.ContainsKey(type)) {
                var Repository = new GenericRepository<T>(_dbContext);
                Repositories.Add(type, Repository);
            }
            return Repositories[type] as IGenericRepository<T>;
        }
    }
}
