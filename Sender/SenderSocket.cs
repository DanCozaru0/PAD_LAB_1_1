using System.Net;
using System.Net.Sockets;

namespace MessageBrokerSockets;

public class SenderSocket
{
    private Socket _socket;
    public bool IsConnected;

    public SenderSocket()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void Connect(string ipAdress, int port)
    {
        _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAdress), port), ConnectedCallback, null);//varianta asincrona
                               //porneste alt fir de ex ce stabileste conexiune si cand e gata, se apeleaza callback, se incepe connect
        Thread.Sleep(5000);
    }

    public void Send(byte[] data)
    {
        try
        {
            _socket.Send(data);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Couldn't send data, {e.Message}");
        }
    }
    private void ConnectedCallback(IAsyncResult ar)
    {
        Console.WriteLine(_socket.Connected ? "Sender connected" : "Error: sender couldn't connect");

        IsConnected = _socket.Connected;
    }
}