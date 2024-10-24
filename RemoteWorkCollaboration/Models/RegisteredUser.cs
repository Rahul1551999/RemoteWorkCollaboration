
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace RemoteWorkCollaboration.Models
{
    public class RegisteredUser : Controller
    {

        public int Id {  get; set; }
       
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } // Store hashed password if using security best practices


        public IActionResult Index()
        {
            return View();
        }
    }
}
