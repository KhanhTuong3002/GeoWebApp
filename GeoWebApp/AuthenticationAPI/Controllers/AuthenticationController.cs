namespace AuthenticationAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login()
    {
        // Thực hiện xác thực người dùng, ví dụ kiểm tra tên đăng nhập và mật khẩu

        // Nếu xác thực thành công, tạo JWT payload
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"), // ID
            new Claim(ClaimTypes.Name, "Khang"), // UserName
            new Claim(ClaimTypes.Email, "Khang@gmail.com"), // Email
            new Claim(ClaimTypes.Role, "admin"), // Role
            new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds().ToString()) // Thời gian hết hạn

        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
