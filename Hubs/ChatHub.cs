using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            var timestamp = DateTimeOffset.Now.ToString("HH:mm:ss");
            await Clients.All.SendAsync("ReceiveMessage", user, message, timestamp);
        }
    }
}
