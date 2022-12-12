using Microsoft.Azure.Devices.Client;
using System.Text;

public class Program {

    static string ? IOT_HUB_DEVICE_CONN_STRING;
    //static IotHubDeviceClient deviceClient;
    static int messageCount = 0;

    const string IOT_HUB_DEVICE = "raspi-dotnet";

    const TransportType IOT_HUB_PROTOCOL = TransportType.Mqtt;

    static void Main(string[] args) {

        IOT_HUB_DEVICE_CONN_STRING = Environment.GetEnvironmentVariable("IOT_HUB_DEVICE_CONN_STRING");

        try {
            while(true) {

                var random = new Random();
                int randomTempeture = random.Next(20, 35);
                int randomHumidity = random.Next(50, 100);

                messageCount ++;

                Console.WriteLine("Sending message...");
                var ret = enviarMensajeIoTHub(randomTempeture, randomHumidity);
                ret.Wait();
                Console.WriteLine("Message was sent!\n");
                Thread.Sleep(1000);
            }
        }
        catch (Exception e) {
            System.Diagnostics.Debug.WriteLine("Error : " + e.Message);
            Console.WriteLine("Error : " + e.Message);
        }
    }

    static async Task enviarMensajeIoTHub(int temperature, int humidity) {

        var deviceClient = DeviceClient.CreateFromConnectionString(IOT_HUB_DEVICE_CONN_STRING, IOT_HUB_PROTOCOL);//i tried other trsnsports
        try {

            var payload = "{" +
                "\"messageId\":"+  "," +
                "\"deviceId\":" + "\"Raspi .NET\"," +
                "\"temperature\":" + temperature.ToString() + "," +
                "\"humidity\":" + humidity.ToString() +                
                "}";

                // Sending message: {"messageId":1,"deviceId":"Raspberry Pi Web Client","temperature":3 ,"humidity":6}

            var msg = new Message(Encoding.UTF8.GetBytes(payload));
            Console.WriteLine(payload);
            await deviceClient.SendEventAsync(msg);
        }
        catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine("!!!! " + ex.Message);
            Console.WriteLine("!!!! " + ex.Message);
        }
    }
}