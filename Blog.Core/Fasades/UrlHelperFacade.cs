using Blog.Abstractions.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Core.Fasades
{
    public class UrlHelperFacade : IUrlHelperFacade
    {
        public string Action(string actionName, string controllerName, object routeValues, string protocol)
        {
            
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return url.Action(actionName, controllerName, routeValues, protocol);
        }

        public string GetUrlScheme()
        {
            return HttpContext.Current.Request.Url.Scheme;
        }
    }
}
