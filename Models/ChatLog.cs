using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SASTCSharpFriendPartService.Models;

[Table("ChatLogs")]
public class ChatLog
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	[Required]
	public string Message { get; set; } = string.Empty;
	[Required]
	public int SessionId { get; set; }
	[ForeignKey("SessionId")]
	[Required]
	public Session Session { get; set; }
	[Required]
	public DateTime CreatedAt { get; set; }
	[Required]
	public DateTime UpdatedAt { get; set; }
}
