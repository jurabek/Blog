using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Abstractions.Repositories
{
    public interface IRepository<T, in TKey> 
        where T : class, new()
    {
        Task<TResult> AddAsync<TResult>(T entity) where TResult : class;

        Task<TResult> UpdateAsync<TResult>(T entity) where TResult : class;

        Task<TResult> DeleteAsync<TResult>(T entity) where TResult : class;

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetAsync(TKey key);

        Task<T> GetByNameAsync(string name);

        TResult Add<TResult>(T entity) where TResult : class;

        TResult Update<TResult>(T entity) where TResult : class;

        TResult Delete<TResult>(T entity) where TResult : class;

        IEnumerable<T> GetAll();

        T Get(TKey key);

        T GetByName(string name);
    }
}
