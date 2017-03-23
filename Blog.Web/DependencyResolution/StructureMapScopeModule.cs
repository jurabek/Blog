namespace Blog.Web.DependencyResolution {
    using System.Web;

    using Blog.Web.App_Start;

    using StructureMap.Web.Pipeline;

    public class StructureMapScopeModule : IHttpModule {
        #region Public Methods and Operators

        public void Dispose() {
        }

        public void Init(HttpApplication context) {
            context.BeginRequest += (sender, e) => StructuremapBootstrapper.StructureMapDependencyScope.CreateNestedContainer();
            context.EndRequest += (sender, e) => {
                HttpContextLifecycle.DisposeAndClearAll();
                StructuremapBootstrapper.StructureMapDependencyScope.DisposeNestedContainer();
            };
        }

        #endregion
    }
}