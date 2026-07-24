using Microsoft.EntityFrameworkCore;

/// <summary>
/// 为 SASTCSharpFriendPartService 提供数据库上下文。
/// </summary>
/// <param name="options"></param>
public class FriendServiceContext(DbContextOptions<FriendServiceContext> options) : DbContext(options)
{
	public DbSet<SASTCSharpFriendPartService.Models.ChatLog> ChatLog { get; set; } = default!;
	public DbSet<SASTCSharpFriendPartService.Models.Friend> Friend { get; set; } = default!;
	public DbSet<SASTCSharpFriendPartService.Models.Session> Session { get; set; } = default!;
}
