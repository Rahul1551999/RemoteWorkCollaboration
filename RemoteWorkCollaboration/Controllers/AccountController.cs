using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RemoteWorkCollaboration.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using RemoteWorkCollaboration.Data;
using System.Threading.Tasks;
using System.Text.Json;

namespace RemoteWorkCollaboration.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context; // Your DbContext for EF Core
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
           
            return View(); // Load the login view explicitly
        }


        // POST: /Account/Login
        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register([FromBody] RegisteredUser model)
        //{
        //    // Log received model
        //    _logger.LogInformation($"Received model: {JsonSerializer.Serialize(model)}");

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Check if passwords match
        //    if (model.Password != model.ConfirmPassword)
        //    {
        //        _logger.LogWarning("Passwords do not match.");
        //        ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
        //        return BadRequest(ModelState);
        //    }

        //    var newUser = new RegisteredUser
        //    {
        //        UserName = model.UserName,
        //        Email = model.Email,
        //        Password = model.Password,
        //        CreatedDate = DateTime.Now
        //    };

        //    try
        //    {
        //        _context.UserTb.Add(newUser);
        //        await _context.SaveChangesAsync();
        //        _logger.LogInformation($"User '{newUser.UserName}' registered successfully.");
        //        return Ok(new { message = "User registered successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error saving user to database: {ex.Message}");
        //        return StatusCode(500, "An error occurred while registering the user.");
        //    }
        //}




        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account"); // Redirect to login page after logout
        }

    }
}
