using InfluxDB.Client.Api.Domain;

public interface ISimulatorService
{
    void SaveDataModel(string inputJsonString);
    string LoadDataModel(string path);
    void GenerateData(DataModel dataModel,int generateTime, int timeInterval);
    object GenerateRandomData(string dataType, double minValue, double maxValue);
    string GenerateRandomString(int length);
    void ShowData();
    List<string> GetAllDataModels(); // nekorisceno
}