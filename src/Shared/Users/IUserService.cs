namespace Squads.Shared.Users;

public interface IUserService
{
    Task<UserResponse.GetIndex> GetIndexAsync(UserRequest.GetIndex request);
    Task<UserResponse.GetDetail> GetDetailByEmailAsync(UserRequest.GetDetail request);
    Task<UserResponse.GetDetail> GetDetailAsync(UserRequest.GetDetail request);
    Task DeleteAsync(UserRequest.Delete request);
    Task<UserResponse.Create> CreateAsync(UserRequest.Create request);
    Task<UserResponse.Edit> EditAsync(UserRequest.Edit request);
    Task<UserResponse.GetDetail> UpdatePricePlan(int userId, int pricePlanId);
    Task<bool> InviteUser(UserRequest.InviteUser request);
    Task<bool> ActivateUser(string token, UserRequest.ActivateUser request);
}
