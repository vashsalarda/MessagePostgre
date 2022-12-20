using Microsoft.EntityFrameworkCore;

namespace MessagePostgre.Models
{
	public class MessageContext : DbContext
	{
		protected readonly IConfiguration Configuration;

		public MessageContext(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			// connect to postgres with connection string from app settings
			options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
		}

		public DbSet<MessageModel> Messages { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MessageModel>(e =>
			{
				e.Property(e => e.Id).HasColumnName("id");
				e.Property(e => e.Message).HasColumnName("message");
				e.Property(e => e.MessageType).HasColumnName("messagetype");
				e.Property(e => e.Date).HasColumnName("date");
				e.Property(e => e.Status).HasColumnName("status");
				e.ToTable("messages");
			});
			
			base.OnModelCreating(modelBuilder);
		}
	}
}