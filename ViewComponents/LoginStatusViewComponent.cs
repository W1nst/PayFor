using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PayFor.Models;
using PayFor.ViewModels;

namespace PayFor.ViewComponents
{
    public class LoginStatusViewComponent : ViewComponent
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginStatusViewComponent(UserManager<User> userManager, SignInManager<User> signInManager  )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var vmUser = Mapper.Map<LoginViewModel>(user);
                return View("LoggedIn", vmUser);
            }
            else
            {
                return View();
            }
        }
    }
}
