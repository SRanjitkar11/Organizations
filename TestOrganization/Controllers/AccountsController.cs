using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using TestOrganization.Data;
using TestOrganization.Models;
using TestOrganization.Models.AccountViewModels;

namespace TestOrganization.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        public AccountsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountsController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Register(string id)
        {
            var organization = _context.Organizations.Find(id);
            var organ = new RegisterViewModel();
            if (organization == null)
            {
                organ = new RegisterViewModel
                {
                    OrganizationId = string.Empty
                };
            }
            else
            {
                organ = new RegisterViewModel
                {
                    OrganizationId = organization.Id
                };
            }

            var roles = (from x in _context.Roles
                         select x).ToList();
            ViewBag.Role = roles;
            return View(organ);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? id)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    NormalizedEmail = model.Email.ToUpper(),
                    UserName = model.UserName,
                    NormalizedUserName = model.UserName.ToUpper()
                };
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                if (result.Succeeded)
                {
                    var userRole = new IdentityUserRole<string>()
                    {
                        UserId = applicationUser.Id,
                        RoleId = model.Role
                    };
                    _context.Add(userRole);
                    await _context.SaveChangesAsync();
                    var orgUser = new OrganizationUser();
                    if(id != null)
                    {
                        orgUser = new OrganizationUser
                        {
                            UserId = applicationUser.Id,
                            OrganizationId = id
                        };
                        _context.OrganizationUsers.Add(orgUser);
                        _context.SaveChanges();
                        _logger.LogInformation("User created a new account with password.");
                        return RedirectToAction("Index", "Organizations");
                    }
                    else
                    {
                        _logger.LogInformation("User created a new account with password.");
                        return RedirectToAction("Index", "Users");
                    }
                    
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Either username or password does not match.");
                    return View(model);
                }

                //var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Email", "Either username or password does not match.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch
            {
                return View();
            }
        }

        #region snippet_GetCurrentUserId
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> GetCurrentUserId()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return user.Id;
        }
        #endregion

        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion

    }
}
