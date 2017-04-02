namespace Blog.Abstractions.Managers
{
    public interface IMappingManager
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}