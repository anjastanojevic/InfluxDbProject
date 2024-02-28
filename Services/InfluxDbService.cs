using System.Dynamic;
using System.Formats.Tar;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

public class InfluxDbService : IInfluxDbService
{
    private readonly InfluxDBClient _influxDbClient;
    private readonly string? _token;
    private readonly string? _bucket;
    private readonly string? _org;
    private readonly string? _url;

    public InfluxDbService(IConfiguration configuration)
    {
        _token = configuration.GetValue<string>("InfluxDb:Token");
        _bucket = configuration.GetValue<string>("InfluxDb:Bucket");
        _org = configuration.GetValue<string>("InfluxDb:Org");
        _url = configuration.GetValue<string>("InfluxDb:Url");
        _influxDbClient = new InfluxDBClient(_url, _token);
    }

    public async Task InsertDataAsync(string measurement, dynamic dataModel)
    {
        using (var writeApi = _influxDbClient.GetWriteApi())
        {
            var point = PointData.Measurement(measurement);
            // point.Tag(dataModel.tag); TAG TREBA DA IMA TagName i TagValue
            point.Tag("proba","probaTag");

            foreach (var field in dataModel.fields)
            {
                point.Field(field.Key, field.Value);
                point.Timestamp(DateTime.UtcNow, WritePrecision.Ns);
            }

            writeApi.WritePoint(point, _bucket, _org); // point -> bucket -> org su argumenti
        }
    }


    public async Task<IEnumerable<FluxTable>> QueryDataAsync(string measurement)
    {
        var query = $"from(bucket: \"newBucket\")"
                  //+ $"|> range(start: -1h)" 
                  + $"|> range(start: 0)"
                  + $"|> filter(fn: (r) => r._measurement == \"{measurement}\")";

        var queryApi = _influxDbClient.GetQueryApi();
        var fluxTables = await queryApi.QueryAsync(query, _org);
        return fluxTables;
    }
}
