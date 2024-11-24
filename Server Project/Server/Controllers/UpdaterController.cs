using CardGame.Data;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server;

[ApiController]
[Route("[controller]")]
public class UpdaterController : ControllerBase
{
    private readonly IUpdaterService m_service;
    
    public UpdaterController(IUpdaterService service) => m_service = service;

    [HttpGet("version")]
    public IntWrapper GetVersion() => m_service.GetVersion();

    [HttpPost("incrementVersion")]
    public IntWrapper IncrementVersion() => m_service.IncrementVersion();
    
    [HttpGet]
    [Route("getNewRules")]
    public async Task<IActionResult> DownloadFile1()
    {
        var fileName = "newRules.txt";   
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);
        
        var memory = new MemoryStream();
        await using (var stream = new FileStream(filePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        
        memory.Position = 0;
        return File(memory, "text/plain", fileName);
    }
}