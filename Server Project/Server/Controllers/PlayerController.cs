using CardGame.Data;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private IDataService m_service;
    
    public PlayerController(IDataService service) => m_service = service;
    
    [HttpGet("{deckSize}/{isEnemy}")]
    public PlayerData GetPlayer([FromRoute] int deckSize, [FromRoute] bool isEnemy) => m_service.GetPlayer(deckSize, isEnemy);

    [HttpGet("increasePopularity/{dataBaseID}")]
    public void IncreasePopularity([FromRoute] int dataBaseID) => m_service.IncreaseCardPopularity(dataBaseID);
}