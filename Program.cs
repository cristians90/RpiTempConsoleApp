using System;
using System.Device.Gpio;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.DHTxx;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RpiTempConsoleApp
{
    class Program
    {
        const string azureIotHubConnString = "HostName=cleveritsensordevicestesthub.azure-devices.net;DeviceId=TH-S0001;SharedAccessKey=qmmxZDQlZ91mOzXKhRF0tCnm3ThmEeUTywuOS/5SUmk=";

        static void Main(string[] args)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

            var deviceClient = DeviceClient.CreateFromConnectionString(azureIotHubConnString);

            if (deviceClient == null)
            {
                Console.WriteLine("Failed to create DeviceClient!");
                return;
            }

            Console.WriteLine("Started");
            Console.WriteLine();

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var task = Task.Run(async () =>
            {
                using (var dht = new Dht11(7, PinNumberingScheme.Board))
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var message = new DeviceMessage
                        {
                            Humidity = dht.Humidity,
                            Temperature = new TemperatureMessage
                            {
                                Celsius = dht.Temperature.Celsius,
                                Kelvin = dht.Temperature.Kelvin,
                                Fahrenheit = dht.Temperature.Fahrenheit
                            }
                        };

                        if (!dht.IsLastReadSuccessful)
                        {
                            continue;
                        }

                        var jsonMessage = JsonConvert.SerializeObject(message, jsonSerializerSettings);
                        var eventMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage));
                        await deviceClient.SendEventAsync(eventMessage);

                        await Task.Delay(3000);
                    }
                }

            }, cancellationToken);

            Console.WriteLine("Press enter to stop the task");
            Console.ReadLine();
            cancellationTokenSource.Cancel();

            Console.WriteLine();
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }
    }
}
