namespace AuthenticationAPI.Controllers;

using AuthenticationAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController(Microsoft.AspNetCore.Identity.UserManager<UserModel> userManager) : ControllerBase
{
    private Microsoft.AspNetCore.Identity.UserManager<UserModel> UserManager { get; } = userManager;

    [HttpPost("login")]
    //public IActionResult Login([FromBody] UserModel loginUser)
    //{
    //    // Thực hiện xác thực người dùng, ví dụ kiểm tra tên đăng nhập và mật khẩu

    //    // Nếu xác thực thành công, tạo JWT payload
        

    //    var claims = new[]
    //    {
    //        new Claim(ClaimTypes.NameIdentifier, "1"), // ID
    //        new Claim(ClaimTypes.Name, "Khang"), // UserName
    //        new Claim(ClaimTypes.Email, "Khang@gmail.com"), // Email
    //        new Claim(ClaimTypes.Role, "admin"), // Role
    //        new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds().ToString()) // Thời gian hết hạn

    //    };

    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey"));
    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //    var token = new JwtSecurityToken(
    //        claims: claims,
    //        expires: DateTime.UtcNow.AddHours(1),
    //        signingCredentials: creds
    //    );

    //    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    //}

    [HttpPost("login")]
    [Authorize]
    public async Task<ActionResult> Authen(UserModel loginUser)
    {
        string role = (await UserManager.GetRolesAsync(loginUser)).ToString();
        return Ok(GenerateToken(loginUser,role));
    }

    private string GenerateToken(UserModel user, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"), // ID
            new Claim(ClaimTypes.Name, "Khang"), // UserName
            new Claim(ClaimTypes.Email, user.Email), // Email
            new Claim(ClaimTypes.Role, role), // Role
            new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds().ToString()) // Thời gian hết hạn

        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(token));
    }
}
