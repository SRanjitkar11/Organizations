using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestOrganization.Data;
using TestOrganization.Models;
using TestOrganization.Models.AccountViewModels;
using TestOrganization.Models.DTO;

namespace TestOrganization.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userList = await _userManager.Users.Where(x => x.IsAdmin == false).ToListAsync();
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.IsAdmin == true)
            {
                List<UserViewModel> users = new List<UserViewModel>();
                foreach (var item in userList)
                {
                    var role = await _context.UserRoles.Where(x => x.UserId == item.Id.ToString()).Select(x => x.RoleId).FirstOrDefaultAsync();
                    var userRole = await _context.Roles.Where(x => x.Id == role).Select(x => x.Name).FirstOrDefaultAsync();
                    var oId = await _context.OrganizationUsers.Where(o => o.UserId == item.Id).Select(o => o.OrganizationId).FirstOrDefaultAsync();
                    var organization = await _context.Organizations.Where(o => o.Id == oId).Select(o => o.Name).FirstOrDefaultAsync();
                    users.Add(new UserViewModel { Id = item.Id, UserName = item.UserName, Email = item.Email, Role = userRole, Organization = organization });
                }
                return View(users);
            }
            var rId = await _context.Roles.Where(x => x.Name == "Admin").Select(x => x.Id).FirstOrDefaultAsync();
            var uId = await _context.UserRoles.Where(x => x.RoleId == rId).Select(x => x.UserId).ToListAsync();
            var user1 = await _context.Users.Where(x => uId.Contains(x.Id)).ToListAsync();
            if (uId.Contains(user.Id))
            {
                var org = await _context.OrganizationUsers.Where(x => x.UserId == user.Id).Select(x => x.OrganizationId).FirstOrDefaultAsync();
                var orgUser = await _context.OrganizationUsers.Where(x => x.OrganizationId == org).Select(x => x.UserId).ToListAsync();
                var orgUserList = await _userManager.Users.Where(x => orgUser.Contains(x.Id)).ToListAsync();
                List<UserViewModel> users = new List<UserViewModel>();
                foreach (var item in orgUserList)
                {
                    var role = await _context.UserRoles.Where(x => x.UserId == item.Id.ToString()).Select(x => x.RoleId).FirstOrDefaultAsync();
                    var userRole = await _context.Roles.Where(x => x.Id == role).Select(x => x.Name).FirstOrDefaultAsync();
                    var oId = await _context.OrganizationUsers.Where(o => o.UserId == item.Id).Select(o => o.OrganizationId).FirstOrDefaultAsync();
                    var organization = await _context.Organizations.Where(o => o.Id == oId).Select(o => o.Name).FirstOrDefaultAsync();
                    users.Add(new UserViewModel { Id = item.Id, UserName = item.UserName, Email = item.Email, Role = userRole, Organization = organization });
                }
                return View(users);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult AddToOrganization(string id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }
            var user = new OrganizationUserDTO
            {
                UserId = id
            };
            var organizations = _context.Organizations.ToList();
            ViewBag.Organizations = organizations;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToOrganization(OrganizationUserDTO model)
        {
            if (ModelState.IsValid)
            {
                var orgUsers = await _context.OrganizationUsers.Where(ou => ou.UserId == model.UserId).ToListAsync();
                if (orgUsers.Any())
                {
                    _context.OrganizationUsers.RemoveRange(orgUsers);
                    _context.SaveChanges();
                }

                var orgUser = new OrganizationUser
                {
                    UserId = model.UserId,
                    OrganizationId = model.OrganizationId
                };

                _context.OrganizationUsers.Add(orgUser);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "An error has occured.");
                return View(model);
            }
        }

        public ActionResult Create()
        {
            return RedirectToAction("Register", "Accounts");
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userId = new UserEditDTO
            {
                Id = id
            };
            var roles = (from x in _context.Roles
                         select x).ToList();
            ViewBag.Role = roles;
            return View(userId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserEditDTO model)
        {
            if (ModelState.IsValid)
            {
                var orgUsers = await _context.UserRoles.Where(ou => ou.UserId == id).ToListAsync();

                if (orgUsers.Any())
                {
                    _context.UserRoles.RemoveRange(orgUsers);
                    _context.SaveChanges();
                }

                var user = new IdentityUserRole<string>()
                {
                    UserId = id,
                    RoleId = model.Role
                };

                _context.UserRoles.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var rId = await _context.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).FirstOrDefaultAsync();
            var role = await _context.Roles.Where(x => x.Id == rId).Select(x => x.Name).FirstOrDefaultAsync();
            var oId = await _context.OrganizationUsers.Where(o => o.UserId == id).Select(o => o.OrganizationId).FirstOrDefaultAsync();
            var organization = await _context.Organizations.Where(o => o.Id == oId).Select(o => o.Name).FirstOrDefaultAsync();
            var userDetail = new UserDeleteDTO()
            {
                Id = id,
                UserName = user.UserName,
                Email = user.Email,
                Role = role,
                Organization = organization
            };

            return View(userDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string id = user.Id;
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return NotFound();
            }
            var rId = await _context.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).FirstOrDefaultAsync();
            var role = await _context.Roles.Where(x => x.Id == rId).Select(x => x.Name).FirstOrDefaultAsync();
            var oId = await _context.OrganizationUsers.Where(o => o.UserId == id).Select(o => o.OrganizationId).FirstOrDefaultAsync();
            var organization = await _context.Organizations.Where(o => o.Id == oId).Select(o => o.Name).FirstOrDefaultAsync();
            var userDetail = new UserDeleteDTO()
            {
                Id = id,
                UserName = user.UserName,
                Email = user.Email,
                Role = role,
                Organization = organization
            };

            return View(userDetail);
        }
    }
}
