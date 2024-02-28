using InfluxDB.Client.Api.Domain;

public interface ISimulatorService
{
    void SaveDataModel(string inputJsonString);
    void GenerateData();
}