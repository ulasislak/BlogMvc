using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.AbstractRepository
{
    public interface IGenericRepository<T> where T : BaseClass
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity,int Id);
        Task DeleteAsync(int Id);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int Id);
    }
}
