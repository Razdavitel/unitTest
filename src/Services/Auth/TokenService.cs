using Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Auth;
public class TokenService
{
    IConfiguration config;
    private static int tokenValidForXDays = 10;
    private static int _activationExpiryHours = 1;

    public TokenService(IConfiguration config)
    {
        this.config = config;
    }

    public string CreateToken(User user, RoleType Role)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, Role.ToString())
        };

        return CreateJwt(claims);
    }

    public string CreateActivationToken(User user)
    {   
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        return CreateJwt(claims, true);
    }

    public int ParseActivationToken(string jwt)
    {
        var validations = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GetKey(),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
        var handler = new JwtSecurityTokenHandler();

        var claims = handler.ValidateToken(jwt, validations, out var securityToken);
        var id = claims.Claims.First().Value;
        return int.Parse(id);
    }

    private SymmetricSecurityKey GetKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));
    }

    private string CreateJwt(List<Claim> claims, bool isActivation=false)
    {
        var key = GetKey();

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var expiry = isActivation? DateTime.Now.AddHours(_activationExpiryHours) : DateTime.Now.AddDays(tokenValidForXDays);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
