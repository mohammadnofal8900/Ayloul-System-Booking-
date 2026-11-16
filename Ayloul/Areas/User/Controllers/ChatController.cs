using Ayloul.Data;
using Ayloul.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ayloul.Areas.User.Controllers
{
    [Area("User")]
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customer = await _userManager.GetUserAsync(User);


            var admin =
                (await _userManager.GetUsersInRoleAsync("Admin"))
                .FirstOrDefault();


            if (admin == null)
                return Content("No Admin Found");



            var chatRoom =
                await _context.ChatRooms
                .FirstOrDefaultAsync(x =>
                    x.Customer_Id == customer.Id
                    &&
                    x.Employee_Id == admin.Id);



            if (chatRoom == null)
            {

                chatRoom = new ChatRoom
                {
                    Customer_Id = customer.Id,
                    Employee_Id = admin.Id
                };


                _context.ChatRooms.Add(chatRoom);

                await _context.SaveChangesAsync();

            }



            chatRoom =
                await _context.ChatRooms
                .Include(x => x.Messages)
                .ThenInclude(x => x.Sender)
                .FirstAsync(x => x.ChatRoom_Id == chatRoom.ChatRoom_Id);



            return View(chatRoom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(int chatRoomId, string message)
        {
            var user = await _userManager.GetUserAsync(User);

            Message msg = new Message
            {
                ChatRoom_Id = chatRoomId,
                Sender_Id = user.Id,
                Message_Text = message
            };

            _context.Messages.Add(msg);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
