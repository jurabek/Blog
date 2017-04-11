using System.Threading.Tasks;

namespace Blog.Abstractions.Providers
{
    public interface IUserPermissionProvider
    {
        Task<bool> CheckUserPermission(string userId, string permission);
    }
}