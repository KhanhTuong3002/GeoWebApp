namespace AuthenticationAPI.Controllers;

using AuthenticationAPI.Models;
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

    //[HttpPost("login")]
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
    public async Task<ActionResult> Authen(UserModel loginUser)
    {
        // Find user by email
        var user = await UserManager.FindByEmailAsync(loginUser.Email);
        if (user == null)
        {
            // User not found
            return Unauthorized("Invalid email or password.");
        }

        // Check if the password is correct
        var isValidPassword = await UserManager.CheckPasswordAsync(user, loginUser.Password);
        if (!isValidPassword)
        {
            // Invalid password
            return Unauthorized("Invalid email or password.");
        }

        // Authentication successful, generate token
        var role = await UserManager.GetRolesAsync(user);
        return Ok(GenerateToken(loginUser, role));
    }

    private string GenerateToken(UserModel user, IList<String> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"), // ID
            new Claim(ClaimTypes.Name, "Khang"), // UserName
            new Claim(ClaimTypes.Email, user.Email), // Email
            new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds().ToString()) // Thời gian hết hạn

        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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
