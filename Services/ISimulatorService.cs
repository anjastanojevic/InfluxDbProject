using InfluxDB.Client.Api.Domain;

public interface ISimulatorService
{
    void SaveDataModel(string inputJsonString);// nekorisceno
    string LoadDataModel(string path);// nekorisceno
    void GenerateData(DataModel dataModel,int generateTime, int timeInterval);
    object GenerateRandomData(string dataType, double minValue, double maxValue);
    string GenerateRandomString(int length);
    List<string> GetAllDataModels(); // nekorisceno
}