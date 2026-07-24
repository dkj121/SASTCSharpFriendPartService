using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SASTCSharpFriendPartService.Hubs;
using SASTCSharpFriendPartService.Models;

namespace SASTCSharpFriendPartService.Endpoints;

/// <summary>
/// 为 SASTCSharpFriendPartService 提供聊天记录（ChatLog）相关的 API 端点。
/// Get：/api/ChatLog - 获取所有聊天记录
/// Get：/api/ChatLog/{id} - 根据 ID 获取特定聊天记录
/// Put：/api/ChatLog/{id} - 更新特定聊天记录
/// Post：/api/ChatLog - 创建新的聊天记录
/// Delete：/api/ChatLog/{id} - 删除特定聊天记录
/// </summary>
public static class ChatLogEndpoints
{
	public static void MapChatLogEndpoints(this IEndpointRouteBuilder routes)
	{
		var group = routes.MapGroup("/api/ChatLog").WithTags(nameof(ChatLog));

		group.MapGet("/", async (FriendServiceContext db) =>
		{
			return await db.ChatLog.ToListAsync();
		})
		.WithName("GetAllChatLogs");

		group.MapGet("/{id}", async Task<Results<Ok<ChatLog>, NotFound>> (int id, FriendServiceContext db) =>
		{
			return await db.ChatLog.AsNoTracking()
				.FirstOrDefaultAsync(model => model.Id == id)
				is ChatLog model
					? TypedResults.Ok(model)
					: TypedResults.NotFound();
		})
		.WithName("GetChatLogById");

		group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, ChatLog chatlog, FriendServiceContext db) =>
		{
			var affected = await db.ChatLog
				.Where(model => model.Id == id)
				.ExecuteUpdateAsync(setters => setters
				.SetProperty(m => m.Message, chatlog.Message)
				.SetProperty(m => m.UpdatedAt, chatlog.UpdatedAt)
		);

			return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
		})
		.WithName("UpdateChatLog");

		group.MapPost("/", async (ChatLog chatlog, FriendServiceContext db, IHubContext<ChatHub> hub) =>
		{
			db.ChatLog.Add(chatlog);
			await db.SaveChangesAsync();
			await hub.Clients.All.SendAsync("ChatLogCreated", chatlog);
			return TypedResults.Created($"/api/ChatLog/{chatlog.Id}", chatlog);
		})
		.WithName("CreateChatLog");

		group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, FriendServiceContext db, IHubContext<ChatHub> hub) =>
		{
			var affected = await db.ChatLog
				.Where(model => model.Id == id)
				.ExecuteDeleteAsync();

			if (affected == 1)
			{
				await hub.Clients.All.SendAsync("ChatLogDeleted", id);
				return TypedResults.Ok();
			}
			return TypedResults.NotFound();
		})
		.WithName("DeleteChatLog");
	}
}
