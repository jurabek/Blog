using StructureMap;

namespace Blog.Core.IoC
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class BaseStructuremapBootstrapper
    {
        public static IContainer Container { get; protected set; }
    }
}
