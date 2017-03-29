namespace Blog.Abstractions.ViewModels
{
    public interface IRegisterUserViewModel
    {
        string Email { get; set; }
        string Password { get; set; }
    }
}