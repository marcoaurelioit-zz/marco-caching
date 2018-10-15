using System.Threading.Tasks;

namespace Marco.Caching
{
    public interface ICache
    {
        bool TryGetValue<T>(string key, out T value) where T : class;
        Task SetAsync<T>(string key, T value) where T : class;
        Task RemoveAsync<T>(string key) where T : class;
    }
}