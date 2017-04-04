using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.Identity;

namespace IdentityPermissionExtension
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    /// <summary>
    /// Permission manager using a IPermissionStore instance to intraction with database.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TPermission"></typeparam>
    /// <typeparam name="TRolePermission"></typeparam>
    public class PermissionManager<TKey, TPermission, TRolePermission> : IPermissionManager
        where TPermission : class, IPermission<TKey, TRolePermission>
        where TRolePermission : IdentityRolePermission<TKey>
        where TKey : IEquatable<TKey>
    {
        protected internal IPermissionStore<TKey, TPermission> Store { get; set; }

        public PermissionManager(IPermissionStore<TKey, TPermission> permissionStore)
        {
            this.Store = permissionStore;
        }

        public virtual async Task InitialConfiguration()
        {
            await this.Store.InitialConfiguration();
        }

        public virtual async Task<IList<string>> GetRolesAsync(string userId, ClaimsPrincipal user = null)
        {
            return await this.Store.GetRolesAsync(userId, user);
        }

        public virtual async Task<TPermission> FindPermissionAsync(string name, long origin, bool isGlobal)
        {
            return await this.Store.FindPermissionAsync(name, origin, isGlobal);
        }

        public virtual async Task<TPermission> CreatePermissionAsync(string name, long origin, string description = null,
            bool isGlobal = false, HttpContextBase httpContext = null)
        {
            return await this.Store.CreatePermissionAsync(name, origin, description, isGlobal, httpContext);
        }

        public virtual async Task<TPermission> CreatePermissionAsync(string name, string description, bool isGlobal)
        {
            return await this.Store.CreatePermissionAsync(name, description, isGlobal);
        }

        public virtual async Task AddToRole(TPermission permission, TKey roleId)
        {
            await this.Store.AddToRole(permission, roleId);
        }

        public virtual async Task RemoveFromRole(TPermission permission, TKey roleId)
        {
            await this.Store.RemoveFromRole(permission, roleId);
        }

        public virtual async Task DeletePermissionAsync(TKey id)
        {
            await this.Store.DeletePermissionAsync(id);
        }

        public Task<IEnumerable<TPermission>> GetAll()
        {
            return this.Store.GetAll();
        }

        /// <summary>
        /// Check weather the permission assigned to the list of roles or not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="roles"></param>
        /// <param name="description"></param>
        /// <param name="isGlobal"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public virtual async Task<bool> CheckPermissionAsync(string name, IList<string> roles, string description = null,
            bool isGlobal = false, HttpContextBase httpContext = null)
        {
            var origin = GetOrigin(httpContext);

            var permission = await FindPermissionAsync(name, origin, isGlobal);

            //TODO: this should not create new CreatePermission
            // ?? await CreatePermissionAsync(name, origin, description, isGlobal, httpContext);

            if (permission == null)
                return await Task.FromResult(false);

            foreach (var role in await this.Store.GetRolesAsync(permission))
            {
                if (roles.Contains(role))
                    return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        /// <summary>
        /// Using the RouteData to find the permission origin.
        /// </summary>
        /// <returns>Return a long hash number. if is zero, the permission defined out of the MVC structure scope.</returns>
        public virtual long GetOrigin(HttpContextBase httpContext = null)
        {
            var mvcOrigin = "";
            if (httpContext != null)
            {
                mvcOrigin += RouteTable.Routes?.GetRouteData(httpContext)?.Values["area"]?.ToString();
                mvcOrigin += RouteTable.Routes?.GetRouteData(httpContext)?.Values["controller"]?.ToString();
                mvcOrigin += RouteTable.Routes?.GetRouteData(httpContext)?.Values["action"]?.ToString();
            }

            if (string.IsNullOrEmpty(mvcOrigin))
                return 0;

            return new ByteEncoder(mvcOrigin).ToLong();
        }

        public virtual async Task<bool> AuthorizePermissionAsync(string name, string description = null,
            bool isGlobal = false)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
                return await Task.FromResult(false);

            var stringUserId = Thread.CurrentPrincipal?.Identity?.GetUserId();
            if (string.IsNullOrEmpty(stringUserId))
                throw new ArgumentNullException(nameof(stringUserId));

            var roles = await this.GetRolesAsync(stringUserId, (ClaimsPrincipal)Thread.CurrentPrincipal);

            //TODO: if is admin otherwise we don't need check permissions
            // in our case admin has all permissions
            if (IsAdmin(roles))
                return await Task.FromResult(true);
           
            return await this.CheckPermissionAsync(name, roles, description, isGlobal);
        }

        private bool IsAdmin(IList<string> roles)
        {           
            if (roles.Contains(nameof(Roles.Administrator)))
                return true;
            return false;
        }

        public virtual async Task<bool> AuthorizePermissionAsync(HttpContextBase httpContext, string name,
            string description = null, bool isGlobal = false)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
                return await Task.FromResult(false);

            var stringUserId = Thread.CurrentPrincipal?.Identity?.GetUserId();
            if (string.IsNullOrEmpty(stringUserId))
                throw new ArgumentNullException(nameof(stringUserId));

            var roles =
                await
                    this.GetRolesAsync(stringUserId, (ClaimsPrincipal)Thread.CurrentPrincipal);

            //NOTE: if is admin otherwise we don't need check permissions
            // in our case admin has all permissions
            if (IsAdmin(roles))
                return await Task.FromResult(true);


            return await this.CheckPermissionAsync(name, roles, description, isGlobal, httpContext);
        }

        public void Dispose()
        {
            this.Store = null;
        }
    }
}