using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.Areas.manage.ViewModels;
using Pustok.DAL;
using Pustok.Models;

namespace Pustok.Areas.manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(PustokDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Create()
        {
            AppUser appUser = new AppUser
            {
                UserName = "SuperAdmin",
            };

            var result = await _userManager.CreateAsync(appUser, "Maqsud_123");

            if (!result.Succeeded)
            {
                return Ok(result.Errors);
            }

            return Ok();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel admin)
        {
            AppUser user = await _userManager.FindByNameAsync(admin.UserName);

            if (user == null)   
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, admin.Password, false, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }

            return RedirectToAction("index", "dashboard");
        }
    }
}
