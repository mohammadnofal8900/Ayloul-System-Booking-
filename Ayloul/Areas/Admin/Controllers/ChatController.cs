using Ayloul.Data;
using Ayloul.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ayloul.Areas.Admin.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    [Area("Admin")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var chats = await _context.ChatRooms
                .Include(c => c.Customer)
                .Include(c => c.Messages)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.LastChats = chats.Take(4).ToList();

            return View(chats);
        }

        [HttpGet]
        public async Task<IActionResult> ChatRoom(int id)
        {
            var chatRoom = await _context.ChatRooms

                .Include(c => c.Customer)

                .Include(c => c.Messages)
                    .ThenInclude(m => m.Sender)

                .FirstOrDefaultAsync(c => c.ChatRoom_Id == id);


            if (chatRoom == null)
                return NotFound();


            return View(chatRoom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(int chatRoomId,string message)
        {

            if (string.IsNullOrWhiteSpace(message))
                return RedirectToAction(nameof(ChatRoom), new { id = chatRoomId });


            var admin = await _userManager.GetUserAsync(User);


            var newMessage = new Message
            {
                ChatRoom_Id = chatRoomId,
                Sender_Id = admin.Id,
                Message_Text = message,
                SentAt = DateTime.Now
            };


            _context.Messages.Add(newMessage);

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(ChatRoom), new { id = chatRoomId });
        }



    }
}
