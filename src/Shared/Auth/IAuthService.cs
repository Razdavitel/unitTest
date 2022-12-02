namespace Squads.Shared.Auth;
public interface IAuthService
{
    Task<AuthResponse.Login> LoginAsync(AuthRequest.Login request);
    Task<AuthResponse.Current> CurrentAsync();
    Task<AuthResponse.Register> RegisterAsync(AuthRequest.Register request);

}
