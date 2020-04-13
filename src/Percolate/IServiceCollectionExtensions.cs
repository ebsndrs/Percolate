using Microsoft.Extensions.DependencyInjection;
using Percolate.ModelBinders;
using System;

namespace Percolate
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPercolate<TPercolateModel>(this IServiceCollection services) where TPercolateModel : PercolateModel
        {
            static void options(PercolateOptions options)
            {
                new PercolateOptions();
            }

            return services.BuildPercolateService<TPercolateModel>(options);
        }

        public static IServiceCollection AddPercolate<TPercolateModel>(this IServiceCollection services, Action<PercolateOptions> options) where TPercolateModel : PercolateModel
        {
            return services.BuildPercolateService<TPercolateModel>(options);
        }

        private static IServiceCollection BuildPercolateService<TPercolateModel>(this IServiceCollection services, Action<PercolateOptions> options) where TPercolateModel : PercolateModel
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
