using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/simulator")]
public class SimulatorController : ControllerBase
{
    private readonly IInfluxDbService _influxDbService;

    public SimulatorController(IInfluxDbService influxDbService)
    {
        _influxDbService = influxDbService;
    }

    [HttpPost]
    public IActionResult GenerateData([FromBody] SimulatorParameters parameters)
    {
        //implementirati logiku za generisanje podataka

        return Ok("");
    }
}
