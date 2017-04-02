using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Fasades;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;

namespace Blog.Core.Repositories
{
    public class RoleRepository : IRepository<IdentityRole, string>
    {
        private readonly IRoleManagerFacade _roleManagerFacade;
        public RoleRepository(IRoleManagerFacade roleManagerFacade)
        {
            _roleManagerFacade = roleManagerFacade;
        }

        public TResult Add<TResult>(IdentityRole entity) where TResult : class
        {
            return _roleManagerFacade.Create(entity) as TResult;
        }

        public Task<TResult> AddAsync<TResult>(IdentityRole entity) where TResult : class
        {
            return Task.FromResult(Add<TResult>(entity));
        }

        public TResult Delete<TResult>(IdentityRole entity) where TResult : class
        {
            return _roleManagerFacade.Delete(entity) as TResult;
        }

        public Task<TResult> DeleteAsync<TResult>(IdentityRole entity) where TResult : class
        {
            return Task.FromResult(Delete<TResult>(entity));
        }

        public IdentityRole Get(string key)
        {
            return _roleManagerFacade.FindByIdAsync(key).Result;
        }

        public IEnumerable<IdentityRole> GetAll()
        {
            return _roleManagerFacade.Roles;
        }

        public Task<IEnumerable<IdentityRole>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        public Task<IdentityRole> GetAsync(string key)
        {
            return _roleManagerFacade.FindByIdAsync(key);
        }

        public IdentityRole GetByName(string name)
        {
            return _roleManagerFacade.FindByNameAsync(name).Result;
        }

        public Task<IdentityRole> GetByNameAsync(string name)
        {
            return _roleManagerFacade.FindByNameAsync(name);
        }

        public TResult Update<TResult>(IdentityRole entity) where TResult : class
        {
            return _roleManagerFacade.Update(entity) as TResult;
        }

        public Task<TResult> UpdateAsync<TResult>(IdentityRole entity) where TResult : class
        {
            return Task.FromResult(Update<TResult>(entity));
        }
    }
}
