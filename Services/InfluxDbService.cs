using InfluxDB.Client;


public class InfluxDbService : IInfluxDbService
{
    private readonly InfluxDBClient _influxDbClient;

    public InfluxDbService(string connectionString)
    {
        _influxDbClient = new InfluxDBClient(connectionString);

    }
    public void Write(Action<WriteApi> action)
    {
        using var write = _influxDbClient.GetWriteApi();
        action(write);
    }

    public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
    {
        var query = _influxDbClient.GetQueryApi();
        return await action(query);
    }
}
