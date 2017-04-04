using System;
using AutoMapper;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Mappings;
using Blog.Model.Entities;

namespace Blog.Core.Managers
{
    public class MappingManager : IMappingManager
    {
        private IUserRolesMapper _userRolesMapper;
        private IRolePermissionsMapper _rolePermissionsMapper;

        public MappingManager(IUserRolesMapper userRolesMapper, IRolePermissionsMapper rolePermissionsMapper)
        {
            _userRolesMapper = userRolesMapper;
            _rolePermissionsMapper = rolePermissionsMapper;
        }

        public IRolePermissionsMapper GetRolePermissionsMapper()
        {
            return _rolePermissionsMapper;
        }

        public IUserRolesMapper GetUserRolesMapper()
        {
            return _userRolesMapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }
    }
}
