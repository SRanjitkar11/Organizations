using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestOrganization.Data;
using TestOrganization.Models;
using TestOrganization.Models.AccountViewModels;
using TestOrganization.Models.DTO;

namespace TestOrganization.Controllers
{
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
        // GET: UsersController
        public ActionResult Index()
        {
            var user = _userManager.Users.Where(x => x.IsAdmin == false).ToList();
            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var item in user)
            {
                var role = _context.UserRoles.Where(x => x.UserId == item.Id.ToString()).Select(x => x.RoleId).FirstOrDefault();
                var userRole = _context.Roles.Where(x => x.Id == role).Select(x => x.Name).FirstOrDefault();
                users.Add(new UserViewModel { Id = item.Id, UserName = item.UserName, Email = item.Email, Role = userRole });
            }
            return View(users);
        }

        // GET: UsersController/Details/5
        [Authorize]
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

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return RedirectToAction("Register", "Accounts");
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
