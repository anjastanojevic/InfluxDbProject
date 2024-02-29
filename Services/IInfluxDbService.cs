using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using InfluxDB.Client.Core.Flux.Domain;

public interface IInfluxDbService
{
    Task InsertDataAsync(string measurement,string tag,string Timestamp, IDictionary<string, object> fields);
    Task<IEnumerable<FluxTable>> QueryDataAsync(string measurement);
}
