using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Squads.Shared.Auth;

namespace Squads.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> logger;
    private readonly IAuthService authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        this.logger = logger;
        this.authService = authService;
    }


    [HttpPost("login"), AllowAnonymous]
    public Task<AuthResponse.Login> Login(AuthRequest.Login req)
    {
        return authService.LoginAsync(req);
    }
    
    [HttpPost("current"), Authorize]
    public Task<AuthResponse.Current> Current()
    {
        return authService.CurrentAsync();
    }

    
    [HttpPost("register"), AllowAnonymous]
    public Task<AuthResponse.Register> Register(AuthRequest.Register req)
    {
        return authService.RegisterAsync(req);
    }
}
