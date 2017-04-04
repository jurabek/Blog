namespace Blog.Abstractions.ViewModels
{
    public interface IIdentityRoleViewModel
    {
        string Id { get; set; }
        bool IsSelected { get; set; }
        string Name { get; set; }
        string Title { get; set; }
    }
}