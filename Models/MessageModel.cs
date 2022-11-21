namespace MessagePostgre.Models
{
	public class MessageModel
	{
		public string? Id { get; set; }
		public string? Message { get; set; }
		public string? MessageType { get; set; }
		public DateTime Date { get; set; }
		public string Status { get; set; } = null!;
	}
}