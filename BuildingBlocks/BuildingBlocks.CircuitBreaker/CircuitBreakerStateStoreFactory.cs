using System.Collections.Concurrent;

namespace MilestoneDeviceAdapter.VideoSequenceExtraction.CircuitBreaker;

public class CircuitBreakerStateStoreFactory
{
    private static readonly ConcurrentDictionary<string, ICircuitBreakerStateStore> stateStores = new();

    public static ICircuitBreakerStateStore GetCircuitBreakerStateStore(string key)
    {
        // The ConcurrentDictionary keeps track of ICircuitBreakerStateStore objects (across threads)
        if (!stateStores.ContainsKey(key))
        {
            stateStores.TryAdd(key, new CircuitBreakerStateStore(key));
        }

        return stateStores[key];
    }
}
