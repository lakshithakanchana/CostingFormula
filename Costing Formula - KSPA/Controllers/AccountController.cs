using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models.ViewModels;
using Costing_Formula___KSPA.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Costing_Formula___KSPA.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
                RedirectToAction("Index", "Home");

            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Helper.RepOrCoordinator));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider = "Google", string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                TempData["ErrorMessage"] = $"Error from external provider: {remoteError}";
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["ErrorMessage"] = "Error loading external login information.";
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                // We need to check if user is active even if they have an external login
                var emailAddress = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                var loggedInUser = await _userManager.FindByEmailAsync(emailAddress);
                if(loggedInUser != null && !loggedInUser.IsActive)
                {
                    await _signInManager.SignOutAsync();
                    TempData["ErrorMessage"] = "Unauthorized or Inactive Account.";
                    return RedirectToAction("Login", new { ReturnUrl = returnUrl });
                }

                TempData["SuccessMessage"] = "Login successful, Welcome!";
                return LocalRedirect(returnUrl);
            }
            
            // If we get here, the user doesn't have a login in AspNetUserLogins yet.
            var email = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null && user.IsActive)
                {
                    // Add the external login to the user
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (addLoginResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        TempData["SuccessMessage"] = "Login successful, Welcome!";
                        return LocalRedirect(returnUrl);
                    }
                }
            }

            // If user not found, or inactive, explicitly deny access
            TempData["ErrorMessage"] = "Unauthorized or Inactive Account.";
            return RedirectToAction("Login", new { ReturnUrl = returnUrl });
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "Signed out successfully, Bye!";
            return RedirectToAction("Login");
        }



    }
}
