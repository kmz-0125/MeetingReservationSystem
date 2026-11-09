using MeetingReservationSystem.Data;
using MeetingReservationSystem.Extensions;
using MeetingReservationSystem.Models;
using MeetingReservationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace MeetingReservationSystem.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<List<SelectListItem>> LoadRoomsAsync()
        {
            var rooms = await _context.Rooms
                .Select(r => new SelectListItem
                {
                    Text = r.RoomName,
                    Value = r.RoomId.ToString()
                })
                .ToListAsync();

            return rooms;
        }

        // GET: /Reservations/Index
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .ToListAsync();

            return View(reservations);
        }

        // GET: /Reservations/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateReservationViewModel();
            model.Rooms = await LoadRoomsAsync();

            return View(model);
        }

        // POST: /Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Rooms = await LoadRoomsAsync();
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            if (userId is null) return Challenge();

            // StartTime EndTimeのnullチェック、DateTimeにキャスト
            if (model.StartTime is not DateTime start || model.EndTime is not DateTime end)
            {
                model.Rooms = await LoadRoomsAsync();
                return View(model);
            }

            if (start >= end)
            {
                ModelState.AddModelError("EndTime", "終了時刻は開始時刻より後にしてください");
                model.Rooms = await LoadRoomsAsync();
                return View(model);
            }

            bool exists = await _context.Rooms
                .AnyAsync(r => r.RoomId == model.RoomId);

            if (!exists)
            {
                ModelState.AddModelError("RoomId", "会議室が存在しません");
                model.Rooms = await LoadRoomsAsync();
                return View(model);
            }

            var reservation = new Reservation
            {
                UserId = userId,
                Title = model.Title,
                RoomId = model.RoomId,
                StartTime = TimeHelper.ToUtcFromJst(start),
                EndTime = TimeHelper.ToUtcFromJst(end),
                ParticipantCount = model.ParticipantCount,
                Announcement = model.Announcement,
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
