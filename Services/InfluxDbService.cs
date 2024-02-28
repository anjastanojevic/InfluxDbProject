using InfluxDB.Client;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

public class InfluxDbService : IInfluxDbService
{
    private readonly InfluxDBClient _influxDbClient;
    private readonly string _token;
    private readonly string _bucket;
    private readonly string _org;
     

    public InfluxDbService(/* string connectionString, */ IConfiguration configuration)
    {
        // _influxDbClient = new InfluxDBClient(connectionString);
        _token = configuration.GetValue<string>("InfluxDB:Token");
        _bucket = configuration.GetValue<string>("InfluxDb:Bucket");
        _org = configuration.GetValue<string>("InfluxDb:Org");
    }

    public async Task InsertDataAsync(string measurement, IDictionary<string, object> fields)
    {
        // using (var writeApi = _influxDbClient.GetWriteApi())
        // {
        //     var point = PointData.Measurement(measurement);

        //     foreach (var field in fields)
        //     {
        //         point.AddField(field.Key, field.Value);
        //     }

        //     writeApi.WritePoint(_bucket, _org, point);
        // }
    }

    public async Task<IEnumerable<FluxTable>> QueryDataAsync(string query)
    {
         var queryApi = _influxDbClient.GetQueryApi();

        var fluxTables = await queryApi.QueryAsync(query, _org);

        return fluxTables;
    }
}
