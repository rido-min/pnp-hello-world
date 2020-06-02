using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace my_pnp_device_dotnet
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "HostName=pnp-smr-01.azure-devices.net;DeviceId=my-pnp-device-01;SharedAccessKey=tAfggTdzPx1bGdPDvO3l/6ASwhbTelXrfBLA9sIP/2U=";

            var client = DeviceClient.CreateFromConnectionString(connectionString + ";ModelId=dtmi:rido:mydevicemodel;1", TransportType.Mqtt);
            Console.WriteLine("client connected");

            await client.SetMethodHandlerAsync("reboot", (MethodRequest methodRequest, object userContext) => {
                Console.WriteLine("============REBOOT===========");
                return Task.FromResult(new MethodResponse(200));
            }, client);

            TwinCollection twinProperty = new TwinCollection();
            twinProperty["myProperty"] = ".. in the shiproom";
            await client.UpdateReportedPropertiesAsync(twinProperty);
            Console.WriteLine("Property Updated");

            for (int i = 0; i < 100; i++)
            {
                var message = new Message(Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                           new { temperature = 12.3 + i }
                       )));

                await client.SendEventAsync(message);
                Console.WriteLine("Telemetry sent");

                await Task.Delay(1000);
            }
        }
    }
}
