using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RemoteWorkCollaboration.Models
{
    [Table("UserTb")]

    public class RegisteredUser
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); // Automatically generate a new GUID

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [NotMapped] // Exclude from database
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

}
