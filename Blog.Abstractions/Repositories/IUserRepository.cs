using System.Threading.Tasks;

namespace Blog.Abstractions.Repositories
{
    public interface IUserRepository<T, TKey> : IRepository<T, TKey> 
        where T : class, new()
    {
        Task<TResult> AddAsync<TResult>(T entity, string password) where TResult : class;
    }
}
