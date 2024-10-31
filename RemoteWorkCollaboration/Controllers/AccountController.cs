using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RemoteWorkCollaboration.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using RemoteWorkCollaboration.Data;
using System.Threading.Tasks;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisteredUser model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new RegisteredUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    CreatedDate = DateTime.Now
                };

                try
                {
                    _context.UserTb.Add(newUser);  // Explicitly adding to UserTB
                    _logger.LogInformation("Attempting to save new user to UserTB table.");

                    int result = await _context.SaveChangesAsync();  // Returns the number of affected rows

                    if (result > 0)
                    {
                        _logger.LogInformation($"User '{newUser.UserName}' registered and saved to the database successfully.");
                        TempData["SuccessMessage"] = "User registered successfully!";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        _logger.LogWarning("No rows were affected by SaveChangesAsync. Data may not have been saved.");
                        ModelState.AddModelError(string.Empty, "User registration failed. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error saving user to database: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Failed to save user data. Please try again.");
                }
            }

            return View(model);
        }






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
