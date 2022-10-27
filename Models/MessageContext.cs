using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace MessagePostgre.Models
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options)
            : base(options)
        {
        }

        public DbSet<MessageModel> Messages { get; set; } = null!;
    }
}