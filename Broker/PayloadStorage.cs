using System.Collections.Concurrent;
using Common;
using System.IO;
namespace Broker;

public static class PayloadStorage
{
    private static ConcurrentQueue<Payload> _payloadQueue;

    static PayloadStorage()
    {
        _payloadQueue = new ConcurrentQueue<Payload>();
    }

    public static void Add(Payload payload)
    {
        _payloadQueue.Enqueue(payload);
    }

    public static Payload GetNext()
    {
        _payloadQueue.TryDequeue(out var payload);
        return payload;
    }

    public static bool IsEmpty()
    {
        return _payloadQueue.IsEmpty;
    }
}