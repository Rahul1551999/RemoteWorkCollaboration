using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemoteWorkCollaboration.Data;
using RemoteWorkCollaboration.Models;
using System;
using System.Linq;

using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RemoteWorkCollaboration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserRegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }


        // Utility method for hashing passwords
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisteredUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match." });
            }
            var existingUser = await _context.UserTb.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User already exists." });
            }
            var hashedPassword = HashPassword(model.Password);

            var newUser = new RegisteredUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = hashedPassword,
                CreatedDate = DateTime.Now
            };

            try
            {
                _context.UserTb.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User registered successfully", redirectUrl = "/Account/login" }); // Redirect URL to login page
            }
            catch
            {
                return StatusCode(500, "An error occurred while registering the user.");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the user by email
            var user = await _context.UserTb
                         .Where(u => u.Email == model.Email)
                         .Select(u => new { u.Password, u.UserName })
                         .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound(new { message = "No account found with that email." });
            }

            // Check if the password matches (consider hashing for production)
            var hashedPassword = HashPassword(model.Password);
            if (hashedPassword != user.Password)
            {
                return Unauthorized(new { message = "Invalid password." });
            }


            // Successful login
            return Ok(new { message = "Login successful", redirectUrl = "/Home/index" }); // Redirect URL to dashboard
        }

    }
}
