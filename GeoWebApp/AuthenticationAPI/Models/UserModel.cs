using Microsoft.AspNetCore.Identity;

namespace AuthenticationAPI.Models

{
    public class UserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
