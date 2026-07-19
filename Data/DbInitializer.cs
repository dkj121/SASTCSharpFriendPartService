using Microsoft.EntityFrameworkCore;
using SASTCSharpFriendPartService.Models;

namespace SASTCSharpFriendPartService.Data;

public static class DbInitializer
{
	public static async Task InitializeAsync(WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<FriendServiceContext>();

		await db.Database.EnsureCreatedAsync();

		if (await db.Friend.AnyAsync())
			return;

		var now = DateTime.UtcNow;

		// ── Friends ──
		var serval = new Friend
		{
			Name = "Serval",
			Email = "serval@friend.paw",
			Description = "A kind of cat",
			ImgUrl = "res/Serval.png",
			CreatedAt = now,
			UpdatedAt = now
		};
		var fennec = new Friend
		{
			Name = "Fennec",
			Email = "fennec@friend.paw",
			Description = "A kind of fox",
			ImgUrl = "res/Fennec.png",
			CreatedAt = now,
			UpdatedAt = now
		};
		var egret = new Friend
		{
			Name = "Egret",
			Email = "egret@friend.paw",
			Description = "A kind of bird",
			ImgUrl = "res/Egret.jpg",
			CreatedAt = now,
			UpdatedAt = now
		};
		var blackMamba = new Friend
		{
			Name = "Black Mamba",
			Email = "blackmamba@friend.paw",
			Description = "A kind of snake",
			ImgUrl = "res/Black mamba.png",
			CreatedAt = now,
			UpdatedAt = now
		};

		db.Friend.AddRange(serval, fennec, egret, blackMamba);
		await db.SaveChangesAsync();

		// ── Sessions (posts) ──
		var post1 = new Session
		{
			FriendId = serval.Id,
			Title = "Hello everyone!",
			Content = "I'm Serval, the cat. Nice to meet you all! 🐱",
			CreatedAt = now.AddMinutes(-30),
			UpdatedAt = now.AddMinutes(-30)
		};

		var post2 = new Session
		{
			FriendId = fennec.Id,
			Title = "What's for dinner?",
			Content = "Just curious what everyone is eating tonight. I had some tasty insects! 🦊",
			CreatedAt = now.AddMinutes(-20),
			UpdatedAt = now.AddMinutes(-20)
		};

		var post3 = new Session
		{
			FriendId = egret.Id,
			Title = "Beautiful day for flying",
			Content = "The weather is perfect today. Soaring high above the trees! 🐦",
			CreatedAt = now.AddMinutes(-10),
			UpdatedAt = now.AddMinutes(-10)
		};

		db.Session.AddRange(post1, post2, post3);
		await db.SaveChangesAsync();

		// ── ChatLogs (replies) ──
		db.ChatLog.AddRange(
			new ChatLog
			{
				Message = "Serval is so cool! 🐱",
				SenderId = fennec.Id,
				SessionId = post1.Id,
				CreatedAt = now.AddMinutes(-25),
				UpdatedAt = now.AddMinutes(-25)
			},
			new ChatLog
			{
				Message = "I agree! Cats are the best.",
				SenderId = egret.Id,
				SessionId = post1.Id,
				CreatedAt = now.AddMinutes(-15),
				UpdatedAt = now.AddMinutes(-15)
			},
			new ChatLog
			{
				Message = "Fennec, what do foxes eat?",
				SenderId = serval.Id,
				SessionId = post2.Id,
				CreatedAt = now.AddMinutes(-18),
				UpdatedAt = now.AddMinutes(-18)
			},
			new ChatLog
			{
				Message = "Mostly small animals and insects! 🦊",
				SenderId = fennec.Id,
				SessionId = post2.Id,
				CreatedAt = now.AddMinutes(-17),
				UpdatedAt = now.AddMinutes(-17)
			},
			new ChatLog
			{
				Message = "Interesting... I prefer fish myself.",
				SenderId = blackMamba.Id,
				SessionId = post2.Id,
				CreatedAt = now.AddMinutes(-12),
				UpdatedAt = now.AddMinutes(-12)
			},
			new ChatLog
			{
				Message = "Egret, how's the weather up there? 🐦",
				SenderId = serval.Id,
				SessionId = post3.Id,
				CreatedAt = now.AddMinutes(-8),
				UpdatedAt = now.AddMinutes(-8)
			}
		);

		await db.SaveChangesAsync();
	}
}
