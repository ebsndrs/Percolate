using Microsoft.Extensions.DependencyInjection;
using Percolate.ModelBinders;
using System;

namespace Percolate
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPercolate<TPercolateModel>(this IServiceCollection services) where TPercolateModel : PercolateModel
        {
            return services.BuildPercolateServices<TPercolateModel>(options => new PercolateOptions());
        }

        public static IServiceCollection AddPercolate<TPercolateModel>(this IServiceCollection services, Action<PercolateOptions> options) where TPercolateModel : PercolateModel
        {
            return services.BuildPercolateServices<TPercolateModel>(options);
        }

        private static IServiceCollection BuildPercolateServices<TPercolateModel>(this IServiceCollection services, Action<PercolateOptions> options) where TPercolateModel : PercolateModel
        {
            services.AddMvcCore(options =>
            {
                options.Filters.Add<PercolateActionFilter>();
                options.ModelBinderProviders.Insert(0, new QueryModelBinderProvider());
            });

            services.AddScoped<PercolateModel, TPercolateModel>();
            services.AddScoped<IPercolateService, PercolateService>();
            services.Configure(options);

            return services;
        }
    }
}
