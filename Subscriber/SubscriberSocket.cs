using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;

namespace Subscriber;

public class SubscriberSocket
{
    private Socket _socket;
    private string _topic;

    public SubscriberSocket(string topic)
    {
        _topic = topic;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void Connect(string ipAddress, int port)
    {
        _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectedCallback, null);
        Console.WriteLine("Waiting for connection");
    }

    private void ConnectedCallback(IAsyncResult asyncResult)
    {
        if (_socket.Connected)
        {
            Console.WriteLine("Subscriber connected to broker");
            Subscribe();
            StartReceive();
        }
        else
        {
            Console.WriteLine("Subscriber couldn't connect to broker");
        }
    }

    private void StartReceive()
    {
        var connection = new ConnectionInfo();
        connection.Socket = _socket;
        _socket.BeginReceive(connection.Data, 0, connection.Data.Length, 
            SocketFlags.None, ReceiveCallback, connection);
    }

    private void ReceiveCallback(IAsyncResult asyncResult)
    {
        var connectionInfo = asyncResult.AsyncState as ConnectionInfo;
        try
        {
            SocketError response;
            var buffSize = _socket.EndReceive(asyncResult, out response);
            if (response != SocketError.Success) return;
            var payloadBytes = new byte[buffSize];
            Array.Copy(connectionInfo.Data, payloadBytes, payloadBytes.Length);
            PayloadHandler.Handle(payloadBytes);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Can't receive data from broker. {e.Message}");
        }
        finally
        {
            try
            {
                connectionInfo.Socket.BeginReceive(connectionInfo.Data, 0, connectionInfo.Data.Length, 
                    SocketFlags.None, ReceiveCallback, connectionInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                    //can change
                connectionInfo.Socket.Close();
            }
        }
    }

    private void Subscribe()
    {
        var data = Encoding.UTF8.GetBytes(Settings.CODE_WORD_SUBS + _topic);
        Send(data);
    }
    private void Unsubscribe()
    {
        var data = Encoding.UTF8.GetBytes(Settings.CODE_WORD_SUBS + _topic);
        Send(data);
    }
    private void Send(byte[] data)
    {
        try
        {
            _socket.Send(data);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Couldn't send data. {e.Message}");
        }
    }
}