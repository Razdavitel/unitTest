namespace Squads.Shared.Auth;

public static class AuthRequest
{
    public class Login
    {
        public AuthDto.Login User { get; set; }
    }

    public class Register
    {
        public AuthDto.Register User { get; set; }
    }
}
