using AutoMapper;
using Blog.Abstractions.Managers;

namespace Blog.Core.Managers
{
    public class MappingManager : IMappingManager
    {
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }
    }
}
