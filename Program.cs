using System;
using System.Device.Gpio;
using System.Threading;
using Iot.Device.CpuTemperature;

namespace RpiTempConsoleApp
{
    class Program
    {
        static CpuTemperature temperature = new CpuTemperature();
        static GpioController controller = new GpioController();

        static void Main(string[] args)
        {
            controller.OpenPin(22, PinMode.Output);

            while (true)
            {
                if (temperature.IsAvailable)
                {
                    Console.WriteLine($"The CPU temperature is {temperature.Temperature.Celsius}");
                }

                controller.Write(22, PinValue.High);

                Thread.Sleep(2000);

                controller.Write(22, PinValue.Low);

                Thread.Sleep(2000);
            }
        }

    }
}
