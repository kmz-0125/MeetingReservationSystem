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
    }
}
