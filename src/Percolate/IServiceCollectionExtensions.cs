using Microsoft.Extensions.DependencyInjection;
using System;

namespace Percolate
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPercolate(this IServiceCollection services)
        {
            static void options(PercolateOptions options) => new PercolateOptions();
            return services.BuildPercolateService(options);
        }

        public static IServiceCollection AddPercolate(this IServiceCollection services, Action<PercolateOptions> options)
        {
            return services.BuildPercolateService(options);
        }

        private static IServiceCollection BuildPercolateService(this IServiceCollection services, Action<PercolateOptions> options)
        {
            services.AddMvcCore(options => options.Filters.Add<PercolateActionFilter>());

            return services;
        }
    }
}
