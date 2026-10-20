using MeetingReservationSystem.Data;
using MeetingReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingReservationSystem.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Departments/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments.ToListAsync();

            return View(departments);
        }

        // Get: /Departments/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Post: /Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 部署ID重複チェック
            bool isDuplicatedId = await _context.Departments
              .AnyAsync(department => department.DepartmentId == model.DepartmentId);
            if (isDuplicatedId)
            {
                ModelState.AddModelError("DepartmentId", "登録されている部署IDです");
            }

            // 部署名重複チェック
            bool isDuplicatedName = await _context.Departments
              .AnyAsync(department => department.DepartmentName == model.DepartmentName);
            if (isDuplicatedName)
            {
                ModelState.AddModelError("DepartmentName", "登録されている部署名です");
            }

            // 登録済みの部署ID・部署名があった場合
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 登録の重複なしの場合
            _context.Departments.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: /Departments/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var department = await _context.Departments
                 .FirstOrDefaultAsync(department => department.DepartmentId == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // Post: /Departments/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Department model)
        {
            if (id != model.DepartmentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(department => department.DepartmentId == id);

            if (department == null)
            {
                return NotFound();
            }

            // 部署名の重複確認(自分自身を除く)
            bool isDuplicatedName = await _context.Departments
              .AnyAsync(department => department.DepartmentName == model.DepartmentName && id != department.DepartmentId);

            // 同一名称の部署あり
            if (isDuplicatedName)
            {
                ModelState.AddModelError("DepartmentName", "登録されている部署名です");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 同一名称の部署なし
            department.DepartmentName = model.DepartmentName;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: /Departments/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var department = await _context.Departments
                 .FirstOrDefaultAsync(department => department.DepartmentId == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: /Departments/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(department => department.DepartmentId == id);

            if (department == null)
            {
                return NotFound();
            }

            // 所属人数の確認
            var usersCount = await _context.Users.CountAsync(
                user => user.DepartmentId == id);
            if (usersCount > 0)
            {
                ModelState.AddModelError(string.Empty, "所属者がいるため削除はできません");

                // メソッド名がDeleteConfirmedのためDeleteを指定する必要がある
                return View("Delete", department);
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
