using Blog.Abstractions.Facades;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Abstractions.ViewModels;
using System;
using Microsoft.AspNet.Identity;

namespace Blog.Core.Repositories
{
    public class UserRepository : IUserRepository<User, string>
    {
        private readonly IUserManagerFacade<User> _userManagerFacade;

        public UserRepository(IUserManagerFacade<User> userManagerFacade)
        {
            _userManagerFacade = userManagerFacade;
        }

        public TResult Add<TResult>(User entity) where TResult : class
        {
            return _userManagerFacade.CreateAsync(entity).Result as TResult;
        }

        public async Task<TResult> AddAsync<TResult>(User entity) where TResult : class
        {
            return await _userManagerFacade.CreateAsync(entity) as TResult;
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
            return _userManagerFacade.FindByIdAsync(key).Result;
        }

        public IEnumerable<User> GetAll()
        {
            return _userManagerFacade.Users;
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        public async Task<User> GetAsync(string key)
        {
            return await _userManagerFacade.FindByIdAsync(key);
        }

        public User GetByName(string name)
        {
            return _userManagerFacade.FindByNameAsync(name).Result;
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _userManagerFacade.FindByNameAsync(name);
        }

        public TResult Update<TResult>(User entity) where TResult : class
        {
            return _userManagerFacade.UpdateAsync(entity).Result as TResult;
        }

        public Task<TResult> UpdateAsync<TResult>(User entity) where TResult : class
        {
            return _userManagerFacade.UpdateAsync(entity) as Task<TResult>;
        }

        public async Task<TResult> UpdateUserRoles<TResult, TRoleViewModel>(IEditRoleViewModel<TRoleViewModel> model)
            where TResult : class
            where TRoleViewModel : IIdentityRoleViewModel
        {
            try
            {
                var selectedRoles = model.Roles.Where(ir => ir.IsSelected);
                var unSelectedRoles = model.Roles.Where(ir => !ir.IsSelected);

                var user = await GetAsync(model.UserId);
                var userRoles = user.Roles;

                var rolesToAdd = selectedRoles.Where(r => !userRoles.Select(i => i.RoleId).Contains(r.Id));
                var rolesToRemove = unSelectedRoles.Where(r => userRoles.Select(ur => ur.RoleId).Contains(r.Id));

                if (rolesToAdd.Any())
                    await _userManagerFacade.AddToRolesAsync(model.UserId, rolesToAdd.Select(ir => ir.Name).ToArray());

                if (rolesToRemove.Any())
                    await _userManagerFacade.RemoveFromRolesAsync(model.UserId, rolesToRemove.Select(ir => ir.Name).ToArray());

                return IdentityResult.Success as TResult;
            }
            catch (Exception ex)
            {
                return new IdentityResult(ex.Message) as TResult;
            }
        }
    }
}
