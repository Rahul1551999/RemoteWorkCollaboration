using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemoteWorkCollaboration.Data;
using RemoteWorkCollaboration.Models;
using System;
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

            var newUser = new RegisteredUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password, // Remember to hash the password in production!
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
            var user = await _context.UserTb.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                return NotFound(new { message = "No data found." });
            }

            // Check if the password matches (consider hashing for production)
            if (user.Password != model.Password) // Add password hashing in production
            {
                return Unauthorized(new { message = "Invalid password." });
            }

            // Successful login
            return Ok(new { message = "Login successful", redirectUrl = "/Home/index" }); // Redirect URL to dashboard
        }

    }
}
