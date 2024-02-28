using System.Dynamic;
using Newtonsoft.Json;
public class SimulatorService : ISimulatorService
{

    public void GenerateData()
    {
        throw new NotImplementedException();
    }

    public void SaveDataModel(string inputJsonString)
    {
        // create dynamic object from inputJsonString
        dynamic expando = JsonConvert.DeserializeObject<ExpandoObject>(inputJsonString);
        // string json = Newtonsoft.Json.JsonConvert.SerializeObject(expando);
        string path;
        if (expando == null)
            path = "./Models/Unnamed.json";
        else if (expando is ExpandoObject &&
             !((IDictionary<string, object>)expando).ContainsKey("dataModelName"))
            path = "./Models/Unnamed.json";
        else
            path = $"./Models/${expando.dataModelName}.json";

        try
        {
            File.WriteAllText(path, inputJsonString);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}

