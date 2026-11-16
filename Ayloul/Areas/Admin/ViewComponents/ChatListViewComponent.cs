using Ayloul.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ayloul.ViewComponents
{
    public class ChatListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ChatListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var messages = await _context.Messages
                .Include(m => m.ChatRoom)
                    .ThenInclude(c => c.Customer)
                .Include(m => m.Sender)
                .OrderByDescending(m => m.SentAt)
                .Take(4)
                .ToListAsync();


            return View(messages);
        }
    }
}