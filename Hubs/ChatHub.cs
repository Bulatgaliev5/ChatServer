using ChatServer.Data;
using ChatServer.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatContext _context;

        public ChatHub(ChatContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            var messages = _context.Messages.OrderBy(m => m.Timestamp).ToList();
            foreach (var msg in messages)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", msg.User, msg.Message, msg.Timestamp.ToString("HH:mm:ss"));
            }
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new ChatMessage
            {
                User = user,
                Message = message,
                Timestamp = DateTime.UtcNow
            };
            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", user, message, chatMessage.Timestamp.ToString("HH:mm:ss"));
        }
    }
}
