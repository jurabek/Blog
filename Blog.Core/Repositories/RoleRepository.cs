using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using System.Linq;
using Blog.Abstractions.ViewModels;
using System;
using Blog.Abstractions.Facades;
using Microsoft.AspNet.Identity;

namespace Blog.Core.Repositories
{
    public class RoleRepository : IRoleRepository<IdentityRole, string, IdentityResult>
    {
        private readonly IRoleManagerFacade<IdentityRole> _roleManagerFacade;
        private readonly IPermissionManagerFacade<IdentityPermission> _permissionManager;

        public RoleRepository(IRoleManagerFacade<IdentityRole> roleManagerFacade, IPermissionManagerFacade<IdentityPermission> permissionManager)
        {
            _roleManagerFacade = roleManagerFacade;
            _permissionManager = permissionManager;
        }

        public IdentityResult Add(IdentityRole entity)
        {
            return _roleManagerFacade.Create(entity);
        }

        public Task<IdentityResult> AddAsync(IdentityRole entity)
        {
            return Task.FromResult(Add(entity));
        }

        public IdentityResult Delete(IdentityRole entity)
        {
            return _roleManagerFacade.Delete(entity);
        }

        public Task<IdentityResult> DeleteAsync(IdentityRole entity)
        {
            return Task.FromResult(Delete(entity));
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

        public IdentityResult Update(IdentityRole entity)
        {
            return _roleManagerFacade.Update(entity);
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole entity)
        {
            return Task.FromResult(Update(entity));
        }

        public async Task<IdentityResult> UpdateRolePermissions<TViewModel>(IEditPermissionViewModel<IdentityRole, TViewModel> model) 
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

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return new IdentityResult(ex.Message);
            }
        }
        
    }
}
