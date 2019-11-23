using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Password must be between 10 and 20 characters.")]
        public string Password { get; set; }
    }
}
