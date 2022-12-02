using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Squads.Shared.Workouts;

namespace Server.Controllers;
[ApiController]
[Route("api/[controller]"), Authorize]
public class WorkoutController:ControllerBase
{
    private readonly ILogger<WorkoutController> logger;
    private readonly IWorkoutService workoutService;
    public WorkoutController(ILogger<WorkoutController> logger, IWorkoutService workoutService)
    {
        this.logger = logger;
        this.workoutService = workoutService;
    }

    [HttpGet]
    public Task<WorkoutResponse.GetIndex> GetIndexAsync([FromQuery] WorkoutRequest.GetIndex req)
    {
        return workoutService.GetIndexAsync(req);
    }

    [HttpGet("{WorkoutId}")]
    public Task<WorkoutResponse.GetDetail> GetDetailAsync([FromRoute] WorkoutRequest.GetDetail request)
    {
        return workoutService.GetDetailAsync(request);
    }

    [HttpDelete("{WorkoutId}")]
    public Task DeleteAsync([FromRoute] WorkoutRequest.Delete request)
    {
        return workoutService.DeleteAsync(request);
    }

    [HttpPost]
    public Task<WorkoutResponse.Create> CreateAsync([FromBody] WorkoutRequest.Create request)
    {
        return workoutService.CreateAsync(request);
    }

    [HttpPut]
    public Task<WorkoutResponse.Edit> EditAsync([FromBody] WorkoutRequest.Edit request)
    {
        return workoutService.EditAsync(request);
    }
}
