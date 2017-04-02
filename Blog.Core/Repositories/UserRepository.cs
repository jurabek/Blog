using Blog.Abstractions.Facades;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.Repositories
{
    public class UserRepository : IUserRepository<User, string>
    {
        private IUserManagerFacade _userManagerFacade;

        public UserRepository(IUserManagerFacade userManagerFacade)
        {
            _userManagerFacade = userManagerFacade;
        }

        public TResult Add<TResult>(User entity) where TResult : class
        {
            return _userManagerFacade.CreateAsync(entity as User).Result as TResult;
        }

        public async Task<TResult> AddAsync<TResult>(User entity) where TResult : class
        {
            return await _userManagerFacade.CreateAsync(entity as User) as TResult;
        }

        public async Task<TResult> AddAsync<TResult>(User entity, string password) where TResult : class
        {
            return await _userManagerFacade.CreateAsync(entity, password) as TResult;
        }

        public TResult Delete<TResult>(User entity) where TResult : class
        {
            return _userManagerFacade.DeleteAsync(entity).Result as TResult;
        }

        public Task<TResult> DeleteAsync<TResult>(User entity) where TResult : class
        {
            return _userManagerFacade.DeleteAsync(entity) as Task<TResult>;
        }

        public User Get(string key)
        {
            return _userManagerFacade.FindByIdAsync(key).Result as User;
        }

        public IEnumerable<User> GetAll()
        {
            return _userManagerFacade.Users.Cast<User>();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        public async Task<User> GetAsync(string key)
        {
            return await _userManagerFacade.FindByIdAsync(key) as User;
        }

        public User GetByName(string name)
        {
            return _userManagerFacade.FindByNameAsync(name).Result as User;
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _userManagerFacade.FindByNameAsync(name) as User;
        }

        public TResult Update<TResult>(User entity) where TResult : class
        {
            return _userManagerFacade.UpdateAsync(entity).Result as TResult;
        }

        public Task<TResult> UpdateAsync<TResult>(User entity) where TResult : class
        {
            return _userManagerFacade.UpdateAsync(entity) as Task<TResult>;
        }
    }
}
