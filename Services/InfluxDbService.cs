using InfluxDB.Client;


public class InfluxDbService : IInfluxDbService
{
    private readonly InfluxDBClient _influxDbClient;

    public InfluxDbService(string connectionString)
    {
        _influxDbClient = new InfluxDBClient(connectionString);
    }

    public async Task InsertDataAsync(string measurement, IDictionary<string, object> fields)
    {
        // implementirati logiku za unos podataka 
    }
}
