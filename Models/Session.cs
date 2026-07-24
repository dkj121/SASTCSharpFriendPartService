using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SASTCSharpFriendPartService.Models;

/// <summary>
/// 表示一个会话（Session）实体，包含与朋友（Friend）相关的聊天记录（ChatLog）。
/// </summary>
[Table("Sessions")]
public class Session
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	[Required]
	public int FriendId { get; set; }
	[ForeignKey("FriendId")]
	[Required]
	public Friend Friend { get; set; } = default!;
	[Required]
	public string Title { get; set; } = string.Empty;
	public string Content { get; set; } = string.Empty;
	[Required]
	public List<ChatLog> ChatLogs { get; set; } = [];
	[Required]
	public DateTime CreatedAt { get; set; }
	[Required]
	public DateTime UpdatedAt { get; set; }
}
