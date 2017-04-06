using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Blog.Abstractions.Fasades;
using System.Linq;
using Blog.Abstractions.ViewModels;
using System;
using Microsoft.AspNet.Identity;

namespace Blog.Core.Repositories
{
    public class RoleRepository : IRoleRepository<IdentityRole, string>
    {
        private readonly IRoleManagerFacade<IdentityRole> _roleManagerFacade;
        private readonly IPermissionManagerFacade<IdentityPermission> _permissionManager;

        public RoleRepository(IRoleManagerFacade<IdentityRole> roleManagerFacade, IPermissionManagerFacade<IdentityPermission> permissionManager)
        {
            _roleManagerFacade = roleManagerFacade;
            _permissionManager = permissionManager;
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
            return _roleManagerFacade.Roles.ToList();
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

        public async Task<TResult> UpdateRolePermissions<TResult, TViewModel>(IEditPermissionViewModel<IdentityRole, TViewModel> model) 
            where TResult : class
            where TViewModel : IIdentityPermissionViewModel
        {
            try
            {
                var selectedPermissions = model.Permissions.Where(ip => ip.IsSelected);
                var unSelectedPermissions = model.Permissions.Where(ip => !ip.IsSelected);
                var role = Get(model.RoleId);
                var rolePermissions = role.Permissions;

                var permissionsToAdd = selectedPermissions
                    .Where(p => !rolePermissions.Select(rp => rp.PermissionId).Contains(p.Id));

                var permissionsToDelete = unSelectedPermissions
                    .Where(p => rolePermissions.Select(rp => rp.PermissionId).Contains(p.Id));

                var allPermissions = await _permissionManager.GetAll();

                foreach (var permissionToAdd in permissionsToAdd)
                {
                    var permission = allPermissions.FirstOrDefault(p => p.Id == permissionToAdd.Id);
                    await _permissionManager.AddToRole(permission, role.Id);
                }
                foreach (var permissionToDelete in permissionsToDelete)
                {
                    var permission = allPermissions.FirstOrDefault(p => p.Id == permissionToDelete.Id);
                    await _permissionManager.RemoveFromRole(permission, role.Id);
                }

                return IdentityResult.Success as TResult;
            }
            catch (Exception ex)
            {
                return new IdentityResult(ex.Message) as TResult;
            }
        }
        
    }
}
