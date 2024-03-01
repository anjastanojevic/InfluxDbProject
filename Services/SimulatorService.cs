using System.Dynamic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
public class SimulatorService : ISimulatorService
{
    private readonly IInfluxDbService _influxDbService;
    private string? currentDataModel;
    private DateTime startTime;

    public SimulatorService(IInfluxDbService influxDbService)
    {
        _influxDbService = influxDbService;
    }
    public void SaveDataModel(string inputJsonString)
    {
        dynamic expando = JsonConvert.DeserializeObject<ExpandoObject>(inputJsonString)!;
        string path;
        if (expando == null)
        {
            throw new Exception("Not a JSON Object");
        }
        else if (expando is ExpandoObject &&
             !((IDictionary<string, object>)expando).ContainsKey("dataModelName"))
        {
            path = "./Models/Unnamed.json";
            currentDataModel = "Unnamed";
        }
        else
        {
            path = $"./Models/{expando.dataModelName}.json";
            currentDataModel = expando.dataModelName;
        }

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

    public void GenerateData(DataModel dataModel, int generateTime, int timeInterval)
    {
        var generatedDataNumber = generateTime / timeInterval;
        var tag = dataModel.DataModelTag;
        var dataPoints = new List<Dictionary<string, object>>();
        var dateTimeList = new List<DateTime>();

        if (dataModel != null && dataModel.Fields != null)
        {
            DateTime novaVrednost = DateTime.Parse(dataModel.StartTime);
            for (int i = 0; i < generatedDataNumber; i++)
            {
                var influxFields = new Dictionary<string, object>();

                foreach (var field in dataModel.Fields)
                {
                    string fieldName = field.FieldName;
                    string dataType = field.DataType;
                    double minValue = Convert.ToDouble(field.MinValue);
                    double maxValue = Convert.ToDouble(field.MaxValue);

                    object generatedData = GenerateRandomData(dataType, minValue, maxValue);

                    influxFields.Add(fieldName, generatedData);
                }

                dataPoints.Add(influxFields);
                dateTimeList.Add(novaVrednost);
            }
            _influxDbService.InsertAllDataAsync(dataModel.DataModelName, tag, novaVrednost, dataPoints).Wait();

        }
        else
        {
            Console.WriteLine($"Invalid data model format for {dataModel.DataModelName}");
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

    public List<string> GetAllDataModels()
    {
        List<string> fileNames = new List<string>();
        string folderPath = "./Models";

        try
        {
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);
                
                fileNames.AddRange(files);
            }
            else
            {
                Console.WriteLine($"Folder not found: {folderPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return fileNames;
    }
}

