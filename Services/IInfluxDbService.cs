using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxDB.Client;

public interface IInfluxDbService
{
    //Task InsertDataAsync(string measurement, IDictionary<string, object> fields);
    void Write(Action<WriteApi> action);
    Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action);
}
