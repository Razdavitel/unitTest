namespace Squads.Shared.Sessions;

public interface ISessionService
{
    Task<SessionResponse.GetIndex> GetIndexAsync(SessionRequest.GetIndex request);
    Task<SessionResponse.GetDetail> GetDetailAsync(SessionRequest.GetDetail request);
    Task DeleteAsync(SessionRequest.Delete request);
    Task<SessionResponse.Create> CreateAsync(SessionRequest.Create request);
    Task<SessionResponse.Edit> EditAsync(SessionRequest.Edit request);
    Task<SessionResponse.MutateTrainee> AddTrainee(SessionRequest.MutateTrainee request);
    Task<SessionResponse.MutateTrainee> RemoveTrainee(SessionRequest.MutateTrainee request);
}
