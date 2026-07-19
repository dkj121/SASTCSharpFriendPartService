using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SASTCSharpFriendPartService.Models;

namespace SASTCSharpFriendPartService.Endpoints;

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
				.SetProperty(m => m.Id, chatlog.Id)
				.SetProperty(m => m.Message, chatlog.Message)
				.SetProperty(m => m.SessionId, chatlog.SessionId)
				.SetProperty(m => m.Session, chatlog.Session)
				.SetProperty(m => m.CreatedAt, chatlog.CreatedAt)
				.SetProperty(m => m.UpdatedAt, chatlog.UpdatedAt)
		);

			return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
		})
		.WithName("UpdateChatLog");

		group.MapPost("/", async (ChatLog chatlog, FriendServiceContext db) =>
		{
			db.ChatLog.Add(chatlog);
			await db.SaveChangesAsync();
			return TypedResults.Created($"/api/ChatLog/{chatlog.Id}", chatlog);
		})
		.WithName("CreateChatLog");

		group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, FriendServiceContext db) =>
		{
			var affected = await db.ChatLog
				.Where(model => model.Id == id)
				.ExecuteDeleteAsync();

			return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
		})
		.WithName("DeleteChatLog");
	}
}
