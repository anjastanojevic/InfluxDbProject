using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxDB.Client.Core.Flux.Domain;

public interface IInfluxDbService
{
    Task InsertDataAsync(string measurement, IDictionary<string, object> fields);
    Task<IEnumerable<FluxTable>> QueryDataAsync(string query);
}
