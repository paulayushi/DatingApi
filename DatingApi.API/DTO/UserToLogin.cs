using System.ComponentModel.DataAnnotations;

namespace DatingApi.API.DTO
{
    public class UserToLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}