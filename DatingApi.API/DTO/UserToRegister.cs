using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApi.API.DTO
{
    public class UserToRegister
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage="Password should be in between 4 to 8 character length!")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public UserToRegister()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}