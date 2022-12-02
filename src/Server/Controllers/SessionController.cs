using Microsoft.AspNetCore.Mvc;
using Squads.Shared.Sessions;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class SessionController : ControllerBase
{
    private readonly ILogger<SessionController> logger;
    private readonly ISessionService sessionsService;

    public SessionController(ILogger<SessionController> logger, ISessionService sessionsService)
    {
        this.logger = logger;
        this.sessionsService = sessionsService;
    }

    [HttpGet]
    public Task<SessionResponse.GetIndex> GetIndexAsync([FromQuery] SessionRequest.GetIndex request)
    {
        return sessionsService.GetIndexAsync(request);
    }
    [HttpGet("{SessionId}")]
    public Task<SessionResponse.GetDetail> GetDetailAsync([FromRoute] SessionRequest.GetDetail request)
    {
        return sessionsService.GetDetailAsync(request);
    }

    [HttpDelete("{SessionId}")]
    public Task DeleteAsync([FromRoute] SessionRequest.Delete request)
    {
        return sessionsService.DeleteAsync(request);
    }

    [HttpPost]
    public Task<SessionResponse.Create> CreateAsync([FromBody] SessionRequest.Create request)
    {
        return sessionsService.CreateAsync(request);
    }

    [HttpPut]
    public Task<SessionResponse.Edit> EditAsync([FromBody] SessionRequest.Edit request)
    {
        return sessionsService.EditAsync(request);
    }
    [HttpPut("join")]
    public Task<SessionResponse.MutateTrainee> AddTrainee([FromBody] SessionRequest.MutateTrainee request)
    {
        return sessionsService.AddTrainee(request);
    }
    [HttpPut("leave")]
    public Task<SessionResponse.MutateTrainee> RemoveTrainee([FromBody] SessionRequest.MutateTrainee request)
    {
        return sessionsService.RemoveTrainee(request);
    }
}
