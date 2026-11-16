using Ayloul.Data;
using Ayloul.Models;
using Microsoft.AspNetCore.SignalR;

namespace Ayloul.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;


        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task SendMessage(
            int chatRoomId,
            string senderId,
            string message)
        {

            var newMessage = new Message
            {
                ChatRoom_Id = chatRoomId,
                Sender_Id = senderId,
                Message_Text = message,
                SentAt = DateTime.Now
            };


            _context.Messages.Add(newMessage);

            await _context.SaveChangesAsync();


            var user = await _context.Users
    .FindAsync(senderId);


            await Clients.Group(chatRoomId.ToString())
            .SendAsync(
            "ReceiveMessage",
            user.UserName,
            message
            );
        }


        public async Task JoinRoom(int chatRoomId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                chatRoomId.ToString()
            );
        }
    }
}
