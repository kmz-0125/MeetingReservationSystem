using MeetingReservationSystem.Data;
using MeetingReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingReservationSystem.Controllers
{
    public class RoomsController : Controller
    {
        private readonly AppDbContext _context;

        public RoomsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Rooms/Index
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms.ToListAsync();

            return View(rooms);
        }

        // Get: /Rooms/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Post: /Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 部屋名重複チェック
            bool isDuplicatedName = await _context.Rooms
              .AnyAsync(room => room.RoomName == model.RoomName);
            if (isDuplicatedName)
            {
                ModelState.AddModelError("RoomName", "登録されている部屋名です");
            }

            // 登録済みの部屋があった場合
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 登録の重複なしの場合
            _context.Rooms.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
