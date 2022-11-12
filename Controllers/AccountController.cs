using FlowerSite.Models.IdentityModels;
using FlowerSite.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlowerSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            var user = HttpContext.User;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existUser = await _userManager.FindByNameAsync(model.Username);
            if(existUser != null)
            {
                ModelState.AddModelError("", "Username təkrarlana bilməz");
                return View();
            }

            var user = new User
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Username,
            };

            var result = await _userManager.CreateAsync(user, model.ConfirmPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(); 
            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var existUser = await _userManager.FindByNameAsync(model.Username);

                if (existUser == null)
                {
                    ModelState.AddModelError("", "Username is not correct");
                    return View();
                }

                //var user = new User
                //{
                //    UserName = model.Username
                //};
                var result = await _signInManager.PasswordSignInAsync(existUser, model.Password, false, false);

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Bloklandınız");
                    return View();
                }

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Username or password invalid");
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
