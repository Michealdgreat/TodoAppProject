using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthenticationController(IConfiguration config)
    {
        _config = config;
    }

    public record AuthenticationData(string? UserName, string? Password);
    public record UserData(int Id, string FirstName, string LastName, string Username);

    [HttpPost("token")]
    [AllowAnonymous]
    public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
    {
        var user = validateCredentials(data);

        if (user == null)
        {
            return Unauthorized();
        }

        string token = GenerateToken(user);

        return Ok(token);
    }

    private string GenerateToken(UserData user)
    {
        var secretkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetValue<string>("Authentication:SecretKey")));

       var signingCredentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new();
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.GivenName, user.Id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.FamilyName, user.Id.ToString()));

        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(1),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    private UserData? validateCredentials(AuthenticationData data)
    {
        // THIS IS NOT A PRODUCTION CODE - REPLACE THIS WITH A CALL TO YOUR AUTH SYSTEM

        if (CompareValues(data.UserName, "micheal") && CompareValues(data.Password, "Test123"))
        {
            return new UserData(1, "micheal", "shodamola", data.UserName!);
        }

        if (CompareValues(data.UserName, "dgreat") && CompareValues(data.Password, "Test123"))
        {
            return new UserData(1, "micheal", "dgreat", data.UserName!);
        }

        return null;
    }

    private bool CompareValues(string? actual, string expected)
    {
        if (actual is not null)
        {
            if (actual.Equals(expected))
            {
                return true;
            }
        }

        return false;
    }
}
