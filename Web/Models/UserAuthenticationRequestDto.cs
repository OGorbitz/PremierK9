﻿using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class UserAuthenticationRequestDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
