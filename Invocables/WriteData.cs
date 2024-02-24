using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace INfluxDBproject
{
    public class WriteRunInvocable 
    {
        private readonly InfluxDbService _service;
        private readonly SimulatorParameters _simulatorParams; 

        public WriteRunInvocable(InfluxDbService service, SimulatorParameters simulatorParams)
        {
            _service = service;
            _simulatorParams = simulatorParams; 
        }
        public Task Invoke()
        {
            _service.Write(write =>
            {
                for (int i = 0; i < _simulatorParams.NumberOfDataPoints; i++)
                {
                    double simulatedValue = SimulateValue(_simulatorParams.MinValue, _simulatorParams.MaxValue);
                    
                    var point = PointData.Measurement("speed")
                        .Tag("loc", "Berlin park")
                        .Field("value", simulatedValue)
                        .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

                    write.WritePoint(point, "bucket-proba", "Organizacija");
                }
            });

            return Task.CompletedTask;
        }

        private double SimulateValue(double minValue, double maxValue)
        {
            Random random = new Random(); 
            double range = maxValue - minValue;
            double step = range / _simulatorParams.NumberOfDataPoints;
            int currentStep = random.Next(_simulatorParams.NumberOfDataPoints); 
            double simulatedValue = minValue + step * currentStep;

            return simulatedValue;
        }
    }
}
