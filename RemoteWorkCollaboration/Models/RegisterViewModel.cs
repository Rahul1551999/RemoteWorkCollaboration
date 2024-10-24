﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace RemoteWorkCollaboration.Models
{
    public class RegisterViewModel : Controller
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public IActionResult Index()
        {
            return View();
        }
    }
}
