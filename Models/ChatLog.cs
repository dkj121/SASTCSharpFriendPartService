using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SASTCSharpFriendPartService.Models;

/// <summary>
/// 表示一个聊天记录（ChatLog）实体，包含与发送者（Sender）和会话（Session）相关的信息。
/// </summary>
[Table("ChatLogs")]
public class ChatLog
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	[Required]
	public int SenderId { get; set; }
	[ForeignKey("SenderId")]
	[Required]
	public Friend Sender { get; set; } = default!;
	[Required]
	public string Message { get; set; } = string.Empty;
	[Required]
	public int SessionId { get; set; }
	[ForeignKey("SessionId")]
	[Required]
	public Session Session { get; set; } = default!;
	[Required]
	public DateTime CreatedAt { get; set; }
	[Required]
	public DateTime UpdatedAt { get; set; }
}
