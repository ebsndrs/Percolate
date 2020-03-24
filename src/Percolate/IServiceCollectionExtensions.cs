using Microsoft.Extensions.DependencyInjection;
using System;

namespace Percolate
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPercolate<TPercolateModel>(this IServiceCollection services) where TPercolateModel : PercolateModel
        {
            static void options(PercolateOptions options) => new PercolateOptions();
            return services.BuildPercolateService<TPercolateModel>(options);
        }

        public static IServiceCollection AddPercolate<TPercolateModel>(this IServiceCollection services, Action<PercolateOptions> options) where TPercolateModel : PercolateModel
        {
            return services.BuildPercolateService<TPercolateModel>(options);
        }

        private static IServiceCollection BuildPercolateService<TPercolateModel>(this IServiceCollection services, Action<PercolateOptions> options) where TPercolateModel : PercolateModel
        {
            services.AddMvcCore(options => options.Filters.Add<PercolateActionFilter<TPercolateModel>>());
            services.AddScoped<TPercolateModel>();
            services.AddScoped<IPercolateService<TPercolateModel>, PercolateService<TPercolateModel>>();
            services.Configure(options);

            return services;
        }
    }
}
