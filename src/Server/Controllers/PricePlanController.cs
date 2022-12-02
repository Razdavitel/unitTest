using Squads.Shared.PricePlans;
using Domain.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PricePlanController : ControllerBase
{
    private readonly ILogger<PricePlanController> _logger;
    private readonly IPricePlanService _pricePlanService;

    public PricePlanController(ILogger<PricePlanController> logger, IPricePlanService service)
    {
        _logger = logger;
        _pricePlanService = service;
    }

    [HttpGet]
    public Task<PricePlanResponse.GetIndex> GetIndexAsync([FromQuery] PricePlanRequest.GetIndex request)
    {
        return _pricePlanService.GetIndexAsync(request);
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<PricePlanResponse.GetDetail>> GetDetailAsync([FromRoute] PricePlanRequest.GetDetail request)
    {
        try{
            return await _pricePlanService.GetDetailAsync(request);
        } catch (EntityNotFoundException e) {
            return NotFound(e.Message);
        }
        
    }

    [HttpPost]
    public async Task<ActionResult<PricePlanResponse.Create>> CreatePricePlan([FromBody] PricePlanRequest.Create request)
    {
        var pricePlan = await _pricePlanService.CreateAsync(request);
        return CreatedAtAction("CreatePricePlan", pricePlan);
    }

    [HttpPut]
    public async Task<ActionResult<PricePlanResponse.Edit>> UpdatePricePlan([FromBody] PricePlanRequest.Edit request)
    {
        try {
            return await _pricePlanService.EditAsync(request);
        } catch (EntityNotFoundException e) {
            return NotFound(e.Message);
        }
        
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeletePricePlan([FromRoute] PricePlanRequest.Delete request)
    {
        await _pricePlanService.DeleteAsync(request);
        return NoContent();
    }


}