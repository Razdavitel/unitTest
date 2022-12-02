namespace Squads.Shared.Workouts;
public interface IWorkoutService
{
    Task<WorkoutResponse.GetIndex> GetIndexAsync(WorkoutRequest.GetIndex request);
    Task<WorkoutResponse.GetDetail> GetDetailAsync(WorkoutRequest.GetDetail request);
    Task DeleteAsync(WorkoutRequest.Delete request);
    Task<WorkoutResponse.Create> CreateAsync(WorkoutRequest.Create request);
    Task<WorkoutResponse.Edit> EditAsync(WorkoutRequest.Edit request);
}
