using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Abstractions.Repositories
{
    public interface IRepository<T, TKey> 
        where T : class, new()
    {
        Task<TResult> AddAsync<TResult>(T entity);

        Task<TResult> UpdateAsync<TResult>(T entity);

        Task<TResult> DeleteAsync<TResult>(T entity);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetAsync(TKey key);

        TResult Add<TResult>(T entity);

        TResult Update<TResult>(T entity);

        TResult Delete<TResult>(T entity);

        IEnumerable<T> GetAll();

        T Get(TKey key);
    }
}
