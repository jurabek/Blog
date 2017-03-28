namespace Blog.Abstractions.ViewModels
{
    public interface IResetPasswordViewModel
    {
        string Code { get; set; }
        string Password { get; set; }
        string UserId { get; set; }
    }
}