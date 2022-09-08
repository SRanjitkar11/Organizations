using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestOrganization.Data;
using TestOrganization.Models;
using TestOrganization.Models.DTO;

namespace TestOrganization.Controllers
{
    [Authorize]
    public class OrganizationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        public OrganizationsController(ApplicationDbContext context,
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

        public async Task<IActionResult> Index()
        {
            var adminCheck = _userManager.Users.Where(u => u.UserName == _userManager.GetUserName(User)).Select(x => x.IsAdmin).FirstOrDefault();
            if (adminCheck == true)
            {
                return View(await _context.Organizations.ToListAsync());
            }
            else
            {
                var id = _userManager.Users.Where(u => u.UserName == _userManager.GetUserName(User)).Select(u => u.Id).FirstOrDefault();
                var organizationList = _context.OrganizationUsers.Where(o => o.UserId == id).Select(x => x.OrganizationId).FirstOrDefault();
                return View(await _context.Organizations.Where(x => x.Id == organizationList).ToListAsync());
            }
            
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Code,Email")] OrganizationDTO organizationDTO)
        {
            if (ModelState.IsValid)
            {
                var organization = new Organization()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = organizationDTO.Email,
                    Name = organizationDTO.Name,
                    Code = organizationDTO.Code
                };
                _context.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Code,Email")] Organization organization)
        {
            if (id != organization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Organizations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Organizations'  is null.");
            }
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(string id)
        {
          return (_context.Organizations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
