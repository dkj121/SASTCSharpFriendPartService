using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SASTCSharpFriendPartService.Models;

/// <summary>
/// 表示一个朋友（Friend）实体，包含与会话（Session）和聊天记录（ChatLog）相关的信息。
/// </summary>
[Table("Friends")]
public class Friend
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	[Required]
	public string Name { get; set; } = string.Empty;
	[Required]
	[EmailAddress]
	public string Email { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string ImgUrl { get; set; } = string.Empty;
	[Required]
	public List<Session> Sessions { get; set; } = [];
	[Required]
	public List<ChatLog> ChatLogs { get; set; } = [];
	[Required]
	public DateTime CreatedAt { get; set; }
	[Required]
	public DateTime UpdatedAt { get; set; }
}
