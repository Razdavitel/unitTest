using Squads.Shared.Users;

namespace Squads.Shared.Auth;

public static class AuthResponse
{
    public class Current
    {
        public UserDto.Detail User { get; set; }
    }
    public class Login
    {
        public string Token { get; set; } = string.Empty;
        public UserDto.Index User { get; set; }
    }
    public class Register
    {
        public string Token { get; set; } = string.Empty;
        public UserDto.Index User { get; set; }
    }
}
