using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Abstractions.Facades
{
    public interface IUrlHelperFacade
    {
        string Action(string actionName, string controllerName, object routeValues, string protocol);

        string GetUrlScheme();
    }
}
