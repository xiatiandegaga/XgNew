using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Cloud.Mapster
{
    public static class MapsterHelper
    {
        public static IServiceProvider serviceProvider;

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TDestination>(this object source)
        {
            if(source==default)
                return default;
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<TDestination>(source);
        }

        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<List<TDestination>>(source);
        }

        public static IEnumerable<TDestination> MapToIEnumerable<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            return mapper.Map<IEnumerable<TDestination>>(source);
        }

    }
}
