using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.DHTxx;

namespace RpiTempConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
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
                        var celsius = dht.Temperature.Celsius.ToString();
                        var humidity = dht.Humidity.ToString();

                        if (celsius != "NaN" && humidity != "NaN")
                            Console.WriteLine($"TEMP: {celsius}Cº | HUM: {humidity}%");

                        await Task.Delay(1000);
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
