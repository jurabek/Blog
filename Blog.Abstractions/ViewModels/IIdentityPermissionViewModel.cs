namespace Blog.Abstractions.ViewModels
{
    public interface IIdentityPermissionViewModel
    {
        string Description { get; set; }
        string Id { get; set; }
        bool IsSelected { get; set; }
        string Name { get; set; }
    }
}