using StructureMap;

namespace Blog.Core.IoC
{
    public class BaseStructuremapBootstrapper
    {
        public static IContainer Container { get; protected set; }
    }
}
