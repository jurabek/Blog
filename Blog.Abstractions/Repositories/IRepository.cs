using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Abstractions.Repositories
{
    public interface IRepository<T, in TKey, TResult> 
        where T : class, new()
    {
        Task<TResult> AddAsync(T entity);

        Task<TResult> UpdateAsync(T entity);

        Task<TResult> DeleteAsync(T entity);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetAsync(TKey key);

        Task<T> GetByNameAsync(string name);

        TResult Add(T entity);

        TResult Update(T entity);

        TResult Delete(T entity);

        IEnumerable<T> GetAll();

        T Get(TKey key);

        T GetByName(string name);
    }
}
