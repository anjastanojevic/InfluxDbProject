class Temperature : IFactor
{
    public double Value { get; set; }
    public void ApplyFactor()
    {
        double val = SimulatorParameters.aqiBaseline;
        // if (val > 30) { val *= 0.9; }
    }
}