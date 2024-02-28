using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using InfluxDB.Client.Core.Flux.Domain;

public interface IInfluxDbService
{
    Task InsertDataAsync(string measurement, dynamic dataModel);
    Task<IEnumerable<FluxTable>> QueryDataAsync(string measurement);
}
