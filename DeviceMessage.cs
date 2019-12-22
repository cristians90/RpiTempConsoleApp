namespace RpiTempConsoleApp
{
    public class DeviceMessage
    {
        public double Humidity { get; set; }
        public TemperatureMessage Temperature { get; set; }
    }

    public class TemperatureMessage
    {
        public double Celsius { get; set; }
        public double Kelvin { get; set; }
        public double Fahrenheit { get; set; }
    }
}