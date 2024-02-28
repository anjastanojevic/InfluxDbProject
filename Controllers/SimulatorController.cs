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
    public IActionResult GenerateData([FromBody] SimulatorParameters parameters)
    {
        //implementirati logiku za generisanje podataka

        return Ok();
    }

    [HttpPost]
    [Route("saveDataModel")]
    public IActionResult SaveDataModel([FromBody] object obj)
    {
        string? inputJsonString = obj.ToString();
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
}
