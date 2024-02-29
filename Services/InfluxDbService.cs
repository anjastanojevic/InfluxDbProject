using System.Dynamic;
using System.Formats.Tar;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;


public class InfluxDbService : IInfluxDbService
{
    private readonly InfluxDBClient _influxDbClient;
    private static readonly Random _random = new Random();
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

    public async Task InsertDataAsync(string measurement, string tag, string Timestamp, IDictionary<string, object> fields)
    {
        using (var writeApi = _influxDbClient.GetWriteApi())
        {
            var point = PointData.Measurement(measurement);
            // point.Tag(dataModel.tag); TAG TREBA DA IMA TagName i TagValue
            point = point.Tag("loc", tag);
            point = point.Timestamp(DateTime.Parse(Timestamp), WritePrecision.Ns);
            foreach (var field in fields)
            {
                //point = point.Field("value", _random.NextDouble());
                point = point.Field(field.Key, field.Value);
            }
            writeApi.WritePoint(point, _bucket, _org);

        }
    }
    // public async Task InsertAllDataAsync(string measurement, string tag, List<DateTime> Timestamp, List<Dictionary<string, object>> dataPoints)
    // {
    //     using (var writeApi = _influxDbClient.GetWriteApi())
    //     {
            
    //         int i=0;
    //         foreach (var dataPoint in dataPoints)
    //         {
    //             var point = PointData.Measurement(measurement);
    //             point = point.Tag("loc", tag);

    //             point = point.Timestamp(Timestamp[i], WritePrecision.Ns);

    //             foreach (var field in dataPoint)
    //             {
    //                 point = point.Field(field.Key, field.Value);
    //             }
    //             i++;

    //             writeApi.WritePoint(point, _bucket, _org);
    //         }
    //     }
    // }
    public async Task InsertAllDataAsync(string measurement, string tag, DateTime Timestamp, List<Dictionary<string, object>> dataPoints)
{
    using (var writeApi = _influxDbClient.GetWriteApi())
    {
        
        long increment = 1; // Povećanje vremena između točaka (možete prilagoditi ovo po potrebi)

        foreach (var dataPoint in dataPoints)
        {
            var point = PointData.Measurement(measurement);
            point = point.Tag("loc", tag);
            point = point.Timestamp(Timestamp, WritePrecision.Ns);

            foreach (var field in dataPoint)
            {
                point = point.Field(field.Key, field.Value);
            }

            writeApi.WritePoint(point, _bucket, _org);

            // Inkrementirajte vremenski žig za sljedeću točku kako biste osigurali jedinstvenost vremena
            Timestamp = Timestamp.AddTicks(increment);
        }
    }
}

    public async Task<IEnumerable<FluxRecord>> QueryDataAsync(string measurement)
    {
        var query = $"from(bucket: \"newBucket\")"
                  //+ $"|> range(start: -1h)" 
                  + $"|> range(start: 0)"
                  + $"|> filter(fn: (r) => r._measurement == \"{measurement}\")";

        var queryApi = _influxDbClient.GetQueryApi();
        var fluxTables = await queryApi.QueryAsync(query, _org);

        // Provjerite da li fluxTables sadrži podatke
        if (fluxTables != null && fluxTables.Any())
        {
            // Spojite sve zapise iz svih tablica
            var allRecords = fluxTables.SelectMany(table => table.Records);

            // Provjerite da li postoji barem jedan zapis
            if (allRecords.Any())
            {
                return allRecords;
            }
        }

        // Ako nema podataka, vratite null ili praznu listu, ovisno o zahtjevima vaše aplikacije
        return null; // ili return new List<FluxRecord>();
    }
}
