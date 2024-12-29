using DAL.AbstractRepository;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ConcreteRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseClass
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _entities;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _entities=_context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();  
        }

        public async Task DeleteAsync(int Id)
        {
            var DeleteToItem = await GetByIdAsync(Id);
            _entities.Remove(DeleteToItem);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Id == Id);

        }

        public async Task UpdateAsync(T entity, int Id)
        {
            var UptadeToItem=await GetByIdAsync(Id);    
            entity.CreatedTime = DateTime.Now;
            _entities.Update(UptadeToItem);
        }
    }
}
