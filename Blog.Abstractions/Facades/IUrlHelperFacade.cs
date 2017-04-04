namespace Blog.Abstractions.Facades
{
    public interface IUrlHelperFacade
    {
        string Action(string actionName, string controllerName, object routeValues, string protocol);

        string GetUrlScheme();

        bool IsLocalUrl(string url);
    }
}
