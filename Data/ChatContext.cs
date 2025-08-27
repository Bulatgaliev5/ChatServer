using Microsoft.EntityFrameworkCore;
using ChatServer.Models;

namespace ChatServer.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }
        public DbSet<ChatMessage> Messages { get; set; } = null!;
    }
}
