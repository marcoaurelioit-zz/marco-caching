using Marco.Caching;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CacheServiceCollectionExtensions
    {
        public static IServiceCollection AddMarcoCaching(this IServiceCollection services, CacheConfiguration cacheConfiguration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (cacheConfiguration == null)
                throw new ArgumentNullException(nameof(cacheConfiguration));          

            switch (cacheConfiguration.StorageType.Value)
            {
                case StorageType.Memory:
                    services.AddDistributedMemoryCache();
                    break;
                case StorageType.SqlServer:
                    if (cacheConfiguration.SqlServer == null)
                        throw new InvalidOperationException($"Configuration section '{nameof(CacheConfiguration)}.{nameof(CacheConfiguration.SqlServer)}' not defined.");

                    if (cacheConfiguration.SqlServer
                        .GetType()
                        .GetProperties()
                        .Where(pi => pi.GetValue(cacheConfiguration.SqlServer) is string)
                        .Select(pi => (string)pi.GetValue(cacheConfiguration.SqlServer))
                        .Any(value => string.IsNullOrWhiteSpace(value)))
                        throw new InvalidOperationException($"Some attribute for configuration section '{nameof(CacheConfiguration)}.{nameof(CacheConfiguration.SqlServer)}' not defined.");

                    services.AddDistributedSqlServerCache(options =>
                    {
                        options.ConnectionString = cacheConfiguration.SqlServer.ConnectionString;
                        options.SchemaName = cacheConfiguration.SqlServer.SchemaName;
                        options.TableName = cacheConfiguration.SqlServer.TableName;
                    });
                    break;
                case StorageType.Redis:
                    if (cacheConfiguration.Redis == null)
                        throw new InvalidOperationException($"Configuration section '{nameof(CacheConfiguration)}.{nameof(CacheConfiguration.Redis)}' not defined.");

                    if (cacheConfiguration.Redis
                        .GetType()
                        .GetProperties()
                        .Where(pi => pi.GetValue(cacheConfiguration.Redis) is string)
                        .Select(pi => (string)pi.GetValue(cacheConfiguration.Redis))
                        .Any(value => string.IsNullOrWhiteSpace(value)))
                        throw new InvalidOperationException($"Some attribute for configuration section '{nameof(CacheConfiguration)}.{nameof(CacheConfiguration.Redis)}' not defined.");

                    services.AddDistributedRedisCache(options =>
                    {
                        options.InstanceName = cacheConfiguration.Redis.InstanceName;
                        options.Configuration = cacheConfiguration.Redis.Configuration;
                    });
                    break;
            }

            services.AddSingleton(cacheConfiguration);
            services.AddScoped<ICache, Cache>();

            return services;
        }
    }
}