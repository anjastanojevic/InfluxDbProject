using System.Collections.Generic;
using System.Threading.Tasks;

public interface IInfluxDbService
{
    Task InsertDataAsync(string measurement, IDictionary<string, object> fields);
}
