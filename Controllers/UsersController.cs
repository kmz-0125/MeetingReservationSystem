using MeetingReservationSystem.Data;
using MeetingReservationSystem.Models;
using MeetingReservationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MeetingReservationSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private async Task<List<SelectListItem>> LoadDepartmentsAsync()
        {
            var departments = await _context.Departments
                .Select(d => new SelectListItem
                {
                    Text = d.DepartmentName,
                    Value = d.DepartmentId
                })
                .ToListAsync();

            return departments;
        }

        public UsersController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Users/Register
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel();
            model.Departments = await LoadDepartmentsAsync();

            return View(model);
        }

        // POST: /Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await LoadDepartmentsAsync();
                return View(model);
            }

            bool exists = await _context.Departments
              .AnyAsync(d => d.DepartmentId == model.DepartmentId);
            if (!exists)
            {
                ModelState.AddModelError("DepartmentId", "部署が存在しません");
            }

            if (!ModelState.IsValid)
            {
                model.Departments = await LoadDepartmentsAsync();
                return View(model);
            }

            var applicationUser = new ApplicationUser
            {
                UserName = model.UserName,
                FamilyName = model.FamilyName,
                FirstName = model.FirstName,
                DepartmentId = model.DepartmentId,
            };

            IdentityResult result = await _userManager.CreateAsync(applicationUser, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.Departments = await LoadDepartmentsAsync();
            return View(model);
        }
    }
}
