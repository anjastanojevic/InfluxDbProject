using System.Dynamic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class SimulatorService : ISimulatorService
{
    private readonly InfluxDbService _influxDbService;

    public SimulatorService(InfluxDbService influxDbService)
    {
        _influxDbService = influxDbService;
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

    public string LoadDataModel(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            else
            {
                Console.WriteLine($"File not found: {path}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return null;
        }
    }

    public void GenerateData(string dataModelName)
    {
        string path = $"./Models/{dataModelName}.json";
        string dataModelJson = LoadDataModel(path);

        if (dataModelJson != null)
        {
            dynamic dataModel = JsonConvert.DeserializeObject<ExpandoObject>(dataModelJson);

            if (dataModel != null && dataModel.fields != null)
            {
                var influxFields = new Dictionary<string, object>();

                foreach (var field in dataModel.fields)
                {
                    string fieldName = field.FieldName;
                    string dataType = field.DataType;
                    double minValue = Convert.ToDouble(field.MinValue);
                    double maxValue = Convert.ToDouble(field.MaxValue);

                    object generatedData = GenerateRandomData(dataType, minValue, maxValue);

                    influxFields.Add(fieldName, generatedData);
                }

                _influxDbService.InsertDataAsync("YourMeasurementName", influxFields).Wait();
            }
            else
            {
                Console.WriteLine($"Invalid data model format for {dataModelName}");
            }
        }
        else
        {
            Console.WriteLine($"Failed to load data model: {dataModelName}");
        }
    }

    public object GenerateRandomData(string dataType, double minValue, double maxValue)
    {
        Random random = new Random();

        switch (dataType.ToLower())
        {
            case "string":
                return GenerateRandomString((int)(minValue + random.NextDouble() * (maxValue - minValue)));
            case "int":
                return random.Next((int)minValue, (int)maxValue + 1);
            case "double":
                return minValue + (random.NextDouble() * (maxValue - minValue));
            case "bool":
                return random.Next(0, 2) == 0;
            default:
                throw new ArgumentException($"Unsupported data type: {dataType}");
        }
    }

    public string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

