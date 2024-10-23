using System.Net.Sockets;

namespace Common;

public class ConnectionInfo
{
    public Socket Socket { get; set; }
    public string Address { get; set; }
    public string Topic { get; set; }
    public byte[] Data { get; set; }
    public const int BUFF_SIZE = 1024;

    public ConnectionInfo()
    {
        Data = new byte[BUFF_SIZE];
    }
}