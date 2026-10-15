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

        // GET: /Department/Index
        public async Task<IActionResult> Index()
        {
            // やること: データベースの Departments テーブルから、全部署を取得する
            var departments = await _context.Departments.ToListAsync();

            //返すもの:取得した部署のリストを、Index の画面(ビュー)に渡す
            return View(departments);
        }

        // Get: /Department/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Post: /Department/Create
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
    .AnyAsync(i => i.DepartmentId == model.DepartmentId);
            if (isDuplicatedId)
            {
                ModelState.AddModelError("DepartmentId", "登録されている部署IDです");
            }

            // 部署名重複チェック
            bool isDuplicatedName = await _context.Departments
    .AnyAsync(i => i.DepartmentName == model.DepartmentName);
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
    }
}
