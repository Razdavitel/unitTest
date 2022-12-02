using Squads.Shared.TurnCards;
using Domain.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TurnCardController : ControllerBase
{
    private readonly ILogger<TurnCardController> _logger;
    private readonly ITurnCardService _turnCardService;

    public TurnCardController(ITurnCardService service, ILogger<TurnCardController> logger)
    {
        _logger = logger;
        _turnCardService = service;
    }

    [HttpPost]
    public ActionResult<TurnCardResponse.GetDetail> Create([FromBody] TurnCardRequest.Create request)
    {
        return _turnCardService.Create(request);
    }

    [HttpGet("{Id}")]
    public ActionResult<TurnCardResponse.GetDetail> Get([FromRoute] TurnCardRequest.GetDetail request)
    {
        try
        {
            return _turnCardService.Get(request);
        }
        catch(EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet]
    public ActionResult<TurnCardResponse.GetIndex> GetList([FromQuery] TurnCardRequest.GetIndex request)
    {
        return _turnCardService.GetList(request);
    }

    [HttpDelete("{Id}")]
    public ActionResult Delete([FromRoute] TurnCardRequest.Delete request)
    {
        try
        {
            _turnCardService.Delete(request);
            return NoContent();
        }
        catch(EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{Id}/consumeTurn")]
    public ActionResult<TurnCardResponse.GetDetail> ConsumeTurn([FromRoute] int Id)
    {
        return _turnCardService.ConsumeTurn(Id);
    }
}