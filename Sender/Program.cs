using System.Net.Http.Json;
using System.Text;
using Common;
using MessageBrokerSockets;
using Newtonsoft.Json;

Console.WriteLine("Sender");

var senderSocket = new SenderSocket();
senderSocket.Connect(Settings.BROKER_IP, Settings.BROKER_PORT);

if (senderSocket.IsConnected)
{
    while (true)
    {
        var payload = new Payload();
        Console.Write("Enter topic: ");
        payload.Topic = Console.ReadLine().ToLower();
        Console.Write("Enter message: ");
        payload.Message = Console.ReadLine();

        var payloadString = JsonConvert.SerializeObject(payload);
        var data = Encoding.UTF8.GetBytes(payloadString);
        senderSocket.Send(data);
    }
}

Console.ReadLine();
