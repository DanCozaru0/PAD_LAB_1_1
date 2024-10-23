using System.Net.Http.Json;
using System.Text;
using Common;
using Newtonsoft.Json;

namespace Broker;

public class PayloadHandler
{
    public static void Handle(byte[] payloadArray, ConnectionInfo connection)
    {
        var payloadString = Encoding.UTF8.GetString(payloadArray);
        if (payloadString.StartsWith(Settings.CODE_WORD_SUBS))
        {
            connection.Topic = payloadString.Split(Settings.CODE_WORD_SUBS).LastOrDefault();
            ConnectionsStorage.Add(connection);
        }
        else
        {
            var payload = JsonConvert.DeserializeObject<Payload>(payloadString);
            PayloadStorage.Add(payload);
            Console.WriteLine(payloadString);
        }
    }
}