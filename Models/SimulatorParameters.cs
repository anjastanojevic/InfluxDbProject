namespace InfluxDbProject.Models
{
    public class SimulatorParameters
    {
        public int NumberOfDataPoints { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public static readonly double aqiBaseline = 50;
        public double AQIValue { get; set; }
    }
}