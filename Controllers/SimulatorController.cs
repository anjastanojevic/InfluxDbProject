using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/simulator")]
public class SimulatorController : ControllerBase
{
    private readonly IInfluxDbService _influxDbService;
    private readonly ISimulatorService _simulatorService;

    public SimulatorController(IInfluxDbService influxDbService, ISimulatorService simulatorService)
    {
        _influxDbService = influxDbService;
        _simulatorService = simulatorService;
    }

    [HttpPost]
    [Route("generateData")]
    public IActionResult GenerateData(/* [FromBody] SimulatorParameters parameters */)
    {
        try
        {
            _simulatorService.GenerateData();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500); // internal server error code
        }



        return Ok();
    }

    [HttpPost]
    [Route("saveDataModel")]
    public IActionResult SaveDataModel([FromBody] object obj)
    {
        string inputJsonString = obj.ToString();
        if (String.IsNullOrEmpty(inputJsonString))
            return BadRequest(inputJsonString);
        try
        {
            _simulatorService.SaveDataModel(inputJsonString);
        }
        catch (System.Exception)
        {
            return StatusCode(500);
            throw;
        }

        return Ok();
    }

    [HttpGet]
    [Route("getAllDataModels")]
    public IActionResult GetAllDataModels() // nekorisceno
    {
        var result = _simulatorService.GetAllDataModels();
        try
        {
            return Ok(result);
        }
        catch (System.Exception)
        {
            return BadRequest(result);
        }
    }


    [HttpPost]
    [Route("insertData")]
    public IActionResult InsertData([FromBody] DataModel data)
    {
        string measurement = data.DataModelName;
        string tag = data.DataModelTag;
        var influxFields = new Dictionary<string, object>();

        foreach (var field in data!.Fields)
        {
            string fieldName = field.FieldName;
            string dataType = field.DataType;
            double minValue = Convert.ToDouble(field.MinValue);
            double maxValue = Convert.ToDouble(field.MaxValue);

            object generatedData = _simulatorService.GenerateRandomData(dataType, minValue, maxValue);

            influxFields.Add(fieldName, generatedData);
        }

        try
        {
            _influxDbService.InsertDataAsync(measurement, tag, data.StartTime,influxFields);
        }
        catch (System.Exception)
        {
            return StatusCode(500);
            throw;
        }

        return Ok();
    }

    [HttpPost]
    [Route("queryData")]
    public async Task<IActionResult> QueryData([FromBody] string measurement)
    {
        if (string.IsNullOrEmpty(measurement))
        {
            return BadRequest("Measurement cannot be empty");
        }

        try
        {
            var result = await _influxDbService.QueryDataAsync(measurement);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error querying data: {ex.Message}");
        }
    }
}
