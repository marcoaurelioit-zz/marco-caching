using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Marco.Caching
{
    public class CacheConfiguration
    {
        [Required]
        public StorageType? StorageType { get; set; }

        [Required]
        public bool? Enable { get; set; }

        [Required]
        public int? DefaultExpiration { get; set; }

        public CacheConfigurationSqlServer SqlServer { get; set; }

        public CacheConfigurationRedis Redis { get; set; }

        public IEnumerable<CustomCacheExpiration> CustomExpirations { get; set; }        
    }
}