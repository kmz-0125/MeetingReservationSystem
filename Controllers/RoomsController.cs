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
              .AnyAsync(r => r.RoomName == model.RoomName);
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

        // GET: /Rooms/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.Rooms
                 .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // Post: /Rooms/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room model)
        {
            if (id != model.RoomId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null)
            {
                return NotFound();
            }

            // 部屋名の重複確認(自分自身を除く)
            bool isDuplicatedName = await _context.Rooms
              .AnyAsync(r => r.RoomName == model.RoomName && id != r.RoomId);

            // 同一名称の部屋あり
            if (isDuplicatedName)
            {
                ModelState.AddModelError("RoomName", "登録されている部屋名です");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 同一名称の部屋なし
            room.RoomName = model.RoomName;
            room.Capacity = model.Capacity;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        // GET: /Rooms/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms
                 .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: /Rooms/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null)
            {
                return NotFound();
            }

            // 予約の確認
            var reservationCount = await _context.Reservations.CountAsync(
                rc => rc.RoomId == id);
            if (reservationCount > 0)
            {
                ModelState.AddModelError(string.Empty, "予約があるため削除はできません");

                // メソッド名がDeleteConfirmedのためDeleteを指定する必要がある
                return View("Delete", room);
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
