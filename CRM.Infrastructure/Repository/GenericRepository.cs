using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CRM.domain.Interface;
using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbcontext;
        public GenericRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task Add(T entity)
        {
            await _dbcontext.Set<T>().AddAsync(entity);
        }

        public async Task Delete(Guid id)
        {
            var entity = await _dbcontext.Set<T>().FindAsync(id);
            if(entity != null)
            {
                _dbcontext.Set<T>().Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbcontext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            var response = await _dbcontext.Set<T>().FindAsync(id);
            return response!;
        }

        public void Update(T entity)
        {
             _dbcontext.Set<T>().Update(entity);
        }
    }
}
