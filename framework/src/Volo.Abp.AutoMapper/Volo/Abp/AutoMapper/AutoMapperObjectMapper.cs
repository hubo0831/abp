using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.AutoMapper
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    public class AutoMapperObjectMapper : DefaultObjectMapper
    {
        protected IMapperAccessor MapperAccessor { get; }
        protected AbpAutoMapperOptions Options { get; }

        public AutoMapperObjectMapper(IMapperAccessor mapperAccessor, IOptions<AbpAutoMapperOptions> options, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            MapperAccessor = mapperAccessor;
            Options = options.Value;
        }
        public override TDestination Map<TSource, TDestination>(TSource source, bool onlyAutoMap = false)
        {
            if (onlyAutoMap || Options.OnlyUseAutoMapper)
            {
                return AutoMap<TSource, TDestination>(source);
            }
            return base.Map<TSource, TDestination>(source, onlyAutoMap);
        }
        public override TDestination Map<TSource, TDestination>(TSource source, TDestination destination, bool onlyAutoMap = false)
        {
            if (onlyAutoMap || Options.OnlyUseAutoMapper)
            {
                return AutoMap<TSource, TDestination>(source, destination);
            }
            return base.Map<TSource, TDestination>(source, destination, onlyAutoMap);
        }

        protected override TDestination AutoMap<TSource, TDestination>(object source)
        {
            return MapperAccessor.Mapper.Map<TDestination>(source);
        }

        protected override TDestination AutoMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            return MapperAccessor.Mapper.Map(source, destination);
        }
    }
}
