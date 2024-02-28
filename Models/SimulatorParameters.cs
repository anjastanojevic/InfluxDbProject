
public class SimulatorParameters
{
    public int NumberOfDataPoints { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
    // ostali parametri

    public static readonly double aqiBaseline = 50;
    public double AQIValue { get; set; }
}


