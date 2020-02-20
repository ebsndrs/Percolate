using Microsoft.Extensions.DependencyInjection;
using System;

namespace Percolate
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPercolate(this IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        public static IServiceCollection AddPercolate(this IServiceCollection services, Action<PercolateOptions> options)
        {
            throw new NotImplementedException();
        }

        public static IServiceCollection BuildPercolateService(this IServiceCollection services, Action<PercolateOptions> options)
        {
            throw new NotImplementedException();
        }
    }
}
