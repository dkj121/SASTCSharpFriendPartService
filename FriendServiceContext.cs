using Microsoft.EntityFrameworkCore;

public class FriendServiceContext(DbContextOptions<FriendServiceContext> options) : DbContext(options)
{
	public DbSet<SASTCSharpFriendPartService.Models.ChatLog> ChatLog { get; set; } = default!;
	public DbSet<SASTCSharpFriendPartService.Models.Friend> Friend { get; set; } = default!;
	public DbSet<SASTCSharpFriendPartService.Models.Session> Session { get; set; } = default!;
}
