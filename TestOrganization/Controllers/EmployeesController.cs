using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestOrganization.Data;
using TestOrganization.Models;
using TestOrganization.Models.DTO;

namespace TestOrganization.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string id)
        {
            ViewBag.Id = id;
            return View(await _context.Employees.Where(x => x.OrganizationId == id).ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            var Oid = _context.Employees.Where(x => x.Id == id).Select(x => x.OrganizationId).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.Id = Oid;
            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create(string id)
        {
            var organization = _context.Organizations.Find(id);
            var emp = new EmployeeDTO
            {
                OrganizationId = organization.Id,
            };
            var deg = await _context.Designations.ToListAsync();
            ViewBag.Designation = deg;
            return View(emp);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, EmployeeDTO employeeDTO)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = employeeDTO.FirstName,
                    LastName = employeeDTO.LastName,
                    Address = employeeDTO.Address,
                    OrganizationId = id,
                    Designation = employeeDTO.Designation
                };
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Employees", new { id = id });
            }
            return View(employeeDTO);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            var Oid = _context.Employees.Where(x => x.Id == id).Select(x => x.OrganizationId).FirstOrDefault();
            var emp = new EmployeeEditDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
                Designation = employee.Designation,
                ReportsTo = employee.ReportsTo
            };
            if (emp == null)
            {
                return NotFound();
            }
            ViewBag.Id = Oid;
            var deg = await _context.Designations.ToListAsync();
            ViewBag.Designation = deg;
            var developer = await _context.Designations.Where(x => x.Name == "Developer").FirstAsync();
            var teamLead = await _context.Designations.Where(x => x.Name == "Team Lead").FirstAsync();
            var projectManager = await _context.Designations.Where(x => x.Name == "Project Manager").FirstAsync();

            var rtsTo = new List<Employee>();
            if (employee.Designation == developer.Name)
            {
                rtsTo = await _context.Employees.Where(x => x.Designation == teamLead.Id && x.OrganizationId == Oid).Select(x => x).ToListAsync();
            }
            else if (employee.Designation == teamLead.Name)
            {
                rtsTo = await _context.Employees.Where(x => x.Designation == projectManager.Id && x.OrganizationId == Oid).Select(x => x).ToListAsync();
            }
            else
            {
                rtsTo = await _context.Employees.Where(x => x.Id == null).ToListAsync();
            }
            ViewBag.rtsTo = rtsTo;
            return View(emp);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Address,Designation,ReportsTo")] EmployeeEditDTO eeDTO)
        {
            if (id != eeDTO.Id)
            {
                return NotFound();
            }
            var Oid = _context.Employees.Where(x => x.Id == id).Select(x => x.OrganizationId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = new Employee
                    {
                        Id = id,
                        FirstName = eeDTO.FirstName,
                        LastName = eeDTO.LastName,
                        Address = eeDTO.Address,
                        OrganizationId = Oid,
                        Designation = eeDTO.Designation,
                        ReportsTo = eeDTO.ReportsTo
                    };
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(eeDTO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = Oid });
            }
            return View(eeDTO);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            var Oid = _context.Employees.Where(x => x.Id == id).Select(x => x.OrganizationId).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.Id = Oid;
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            var Oid = _context.Employees.Where(x => x.Id == id).Select(x => x.OrganizationId).FirstOrDefault();
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = Oid });
        }

        private bool EmployeeExists(string id)
        {
          return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
