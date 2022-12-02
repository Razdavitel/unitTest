using Squads.Shared.Subscriptions;
using Domain.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{

    private readonly ILogger<SubscriptionController> _logger;
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService service, ILogger<SubscriptionController> logger)
    {
        _logger = logger;
        _subscriptionService = service;
    }

    [HttpPost]
    public ActionResult<SubscriptionResponse.GetDetail> Create([FromBody] SubscriptionRequest.Create request)
    {
        return _subscriptionService.Create(request);
    }

    [HttpGet("{Id}")]
    public ActionResult<SubscriptionResponse.GetDetail> Get([FromRoute] SubscriptionRequest.GetDetail request)
    {
        try
        {
            return _subscriptionService.Get(request);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        
    }

    [HttpGet]
    public ActionResult<SubscriptionResponse.GetIndex> GetList([FromQuery] SubscriptionRequest.GetIndex request)
    {
        return _subscriptionService.GetList(request);
    }

    [HttpDelete("{Id}")]
    public ActionResult Delete([FromRoute] SubscriptionRequest.Delete request)
    {
        try
        {
            _subscriptionService.Delete(request);
            return NoContent();
        } 
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}