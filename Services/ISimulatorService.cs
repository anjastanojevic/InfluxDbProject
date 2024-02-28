using InfluxDB.Client.Api.Domain;

public interface ISimulatorService
{
    void SaveDataModel(string inputJsonString);
    string LoadDataModel(string path);
    void GenerateData(string dataModelName);
    object GenerateRandomData(string dataType, double minValue, double maxValue);
    string GenerateRandomString(int length);
}