using InfluxDB.Client;


public class InfluxDbService : IInfluxDbService
{
    private readonly InfluxDBClient _influxDbClient;
    private readonly string _token;

    public InfluxDbService(/* string connectionString, */ IConfiguration configuration)
    {
        // _influxDbClient = new InfluxDBClient(connectionString);
        _token = configuration.GetValue<string>("InfluxDB:Token");
    }

    public async Task InsertDataAsync(string measurement, IDictionary<string, object> fields)
    {
        // implementirati logiku za unos podataka 
    }
}
