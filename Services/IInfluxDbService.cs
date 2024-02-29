using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using InfluxDB.Client.Core.Flux.Domain;

public interface IInfluxDbService
{
    Task InsertDataAsync(string measurement,string tag,string Timestamp, IDictionary<string, object> fields);
    
    //Task InsertAllDataAsync(string measurement, string tag,List<DateTime> Timestamp, List<Dictionary<string, object>> dataPoints);
    Task InsertAllDataAsync(string measurement, string tag,DateTime Timestamp, List<Dictionary<string, object>> dataPoints);
    Task<IEnumerable<FluxRecord>> QueryDataAsync(string measurement);
}
