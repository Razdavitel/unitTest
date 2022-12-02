using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Squads.Shared.Users;
using Domain.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> logger;
    private readonly IUserService userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        this.logger = logger;
        this.userService = userService;
    }

    [HttpGet]
    public Task<UserResponse.GetIndex> GetIndexAsync([FromQuery] UserRequest.GetIndex req)
    {
        return userService.GetIndexAsync(req);
    }
    [HttpGet("mail/{Email}")]
    public Task<UserResponse.GetDetail> GetDetailByEmailAsync([FromRoute] UserRequest.GetDetail request)
    {
        return userService.GetDetailByEmailAsync(request);
    }

    [HttpGet("{UserId}")]
    public Task<UserResponse.GetDetail> GetDetailAsync([FromRoute] UserRequest.GetDetail request)
    {
        return userService.GetDetailAsync(request);
    }

    [HttpDelete("{UserId}")]
    public Task DeleteAsync([FromRoute] UserRequest.Delete request)
    {
        return userService.DeleteAsync(request);
    }

    [HttpPost]
    public Task<UserResponse.Create> CreateAsync([FromBody] UserRequest.Create request)
    {
        return userService.CreateAsync(request);
    }


    [HttpPut("{userId}/updatePricePlan/{pricePlanId}")]
    public async Task<ActionResult<UserResponse.GetDetail>> UpdatePricePlan([FromRoute] int userId, [FromRoute] int pricePlanId)
    {
        try{
            return await userService.UpdatePricePlan(userId, pricePlanId);
        } catch (EntityNotFoundException e){
            return NotFound(e.Message);
        }
    }

    [HttpPut]
    public Task<UserResponse.Edit> EditAsync([FromBody] UserRequest.Edit request)
    {
        return userService.EditAsync(request);
    }

    [HttpPost("InviteUser")]
    public async Task<ActionResult> InviteUser([FromBody] UserRequest.InviteUser request)
    {
        await userService.InviteUser(request);
        return NoContent();
    }

    [HttpPut("activate/{token}")]
    public async Task<ActionResult> ActivateUser([FromRoute] string token, [FromBody] UserRequest.ActivateUser request)
    {
        try
        {
            await userService.ActivateUser(token, request);
        }
        catch (SecurityTokenDecryptionFailedException)
        {
            return BadRequest("Invalid Jwt token provided.");
        }
        catch (SecurityTokenExpiredException)
        {
            return BadRequest("Activation link expired.");
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        
        return NoContent(); 
    }
}
