using Blog.Abstractions.Mappings;

namespace Blog.Abstractions.Managers
{
    public interface IMappingManager
    {
        TDestination Map<TSource, TDestination>(TSource source);

        IRolePermissionsMapper GetRolePermissionsMapper();

        IUserRolesMapper GetUserRolesMapper();
    }
}