using System.Dynamic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class SimulatorService : ISimulatorService
{
    private readonly IInfluxDbService _influxDbService;
    private string? currentDataModel;

    public SimulatorService(IInfluxDbService influxDbService)
    {
        _influxDbService = influxDbService;
    }
    public void SaveDataModel(string inputJsonString)
    {
        // create dynamic object from inputJsonString
        dynamic expando = JsonConvert.DeserializeObject<ExpandoObject>(inputJsonString)!;
        // string json = Newtonsoft.Json.JsonConvert.SerializeObject(expando);
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

    public void GenerateData(/* string dataModelName */)
    {
        if (String.IsNullOrEmpty(currentDataModel)) return;
        string dataModelName = currentDataModel;

        string path = $"./Models/{dataModelName}.json";
        string dataModelJson = LoadDataModel(path);

        if (dataModelJson != null)
        {
            dynamic dataModel = JsonConvert.DeserializeObject<ExpandoObject>(dataModelJson);

            if (dataModel != null && dataModel.fields != null)
            {
                // var influxFields = new Dictionary<string, object>();

                foreach (var field in dataModel!.fields)
                {
                    string fieldName = field.fieldName;
                    string dataType = field.dataType;
                    double minValue = Convert.ToDouble(field.minValue);
                    double maxValue = Convert.ToDouble(field.maxValue);

                    object generatedData = GenerateRandomData(dataType, minValue, maxValue);

                    (field as Dictionary<string, object>).Add(fieldName, generatedData);
                }

                _influxDbService.InsertDataAsync("YourMeasurementName", dataModel).Wait();
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

        // OVDE DA SE UBACI GENERISANJE VISE PODATAKA

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
            // Check if the folder exists
            if (Directory.Exists(folderPath))
            {
                // Get all file names in the folder
                string[] files = Directory.GetFiles(folderPath);

                // files = files.Select(x => { Path.GetFileNameWithoutExtension(x); });
                // Add file names to the list
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

    public void ShowData()
    {

    }
}

