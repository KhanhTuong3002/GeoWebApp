using System.IdentityModel.Tokens.Jwt;

namespace WebClient
{
    public class JwtHelper
    {
        //check call to check role OnGet() for admin
        public static string GetUserRoleFromToken(string token)
        {
            var roleClaim = "";
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jsonToken = handler.ReadJwtToken(token);
                roleClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                return roleClaim;
            }
            else
            {
                return roleClaim;
            }
        }
    }
}
