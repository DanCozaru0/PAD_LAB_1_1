using Common;

namespace Broker;

public static class ConnectionsStorage
{
    private static List<ConnectionInfo> _connections;
    private static object _locker;

    static ConnectionsStorage()
    {
        _connections = new List<ConnectionInfo>();
        _locker = new object();
    }

    public static void Add(ConnectionInfo connection)
    {
        lock (_locker)
        {
            _connections.Add(connection);
        }
    }

    public static void Remove(string address)
    {
        lock (_locker)
        {
            _connections.RemoveAll(x => x.Address == address);
        }
    }

    public static List<ConnectionInfo> GetConnectionsByTopic(string topic)
    {
        lock (_locker)
        {
            return _connections.Where(c => c.Topic == topic).ToList();
        }
    }
}