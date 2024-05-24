using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.ViewModel;

namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _Context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext Context)
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _Context=Context;
        }
        public IActionResult Login()
        {
            var response = new LoginVM();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVm)
        {
            if (!ModelState.IsValid) { return View(); }
            var user=await _userManager.FindByEmailAsync(loginVm.EmailAddress);
            if (user != null)
            {
                var passwordCheck= await _userManager.CheckPasswordAsync(user, loginVm.Password);
                if (passwordCheck)
                {
                    var signin= await _signInManager.PasswordSignInAsync(user,loginVm.Password,false,false);
                    if (signin.Succeeded)
                    {
                        return RedirectToAction("Index", "TaskModels");
                    }
                }
                TempData["Error"] = "Wrong Creadential Please try again";
                return View(loginVm);
            }
            TempData["Error"] = "Wrong Creadential Please try again";
            return View(loginVm);
        }

        public IActionResult Register()
        {
            var response = new RegisterVM();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            var newUser = new AppUser()
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            else
            {
                TempData["Error"] = newUserResponse.Errors.FirstOrDefault().Description;
                return View(registerVM);
            }


            return RedirectToAction("Index", "TaskModels");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "TaskModels");
        }
    }
}
