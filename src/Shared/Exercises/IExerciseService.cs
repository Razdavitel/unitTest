namespace Squads.Shared.Exercises;
internal interface IExerciseService
{
    Task<ExerciseResponse.GetIndex> GetIndexAsync(ExerciseRequest.GetIndex request);
    Task<ExerciseResponse.GetDetail> GetDetailAsync(ExerciseRequest.GetDetail request);
    Task DeleteAsync(ExerciseRequest.Delete request);
    Task<ExerciseResponse.Create> CreateAsync(ExerciseRequest.Create request);
    Task<ExerciseResponse.Edit> EditAsync(ExerciseRequest.Edit request);
}
