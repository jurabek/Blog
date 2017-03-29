namespace Blog.Abstractions.ViewModels
{
    public interface IUpdatePasswordViewModel
    {
        string NewPassword { get; set; }
        string OldPassword { get; set; }
    }
}