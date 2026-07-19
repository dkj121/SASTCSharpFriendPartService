using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SASTCSharpFriendPartService.Models;

namespace SASTCSharpFriendPartService.Endpoints;

public static class FriendEndpoints
{
	public static void MapFriendEndpoints(this IEndpointRouteBuilder routes)
	{
		var group = routes.MapGroup("/api/Friend").WithTags(nameof(Friend));

		group.MapGet("/", async (FriendServiceContext db) =>
		{
			return await db.Friend.ToListAsync();
		})
		.WithName("GetAllFriends");

		group.MapGet("/{id}", async Task<Results<Ok<Friend>, NotFound>> (int id, FriendServiceContext db) =>
		{
			return await db.Friend.AsNoTracking()
				.FirstOrDefaultAsync(model => model.Id == id)
				is Friend model
					? TypedResults.Ok(model)
					: TypedResults.NotFound();
		})
		.WithName("GetFriendById");

		group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Friend friend, FriendServiceContext db) =>
		{
			var affected = await db.Friend
				.Where(model => model.Id == id)
				.ExecuteUpdateAsync(setters => setters
				.SetProperty(m => m.Name, friend.Name)
				.SetProperty(m => m.Email, friend.Email)
				.SetProperty(m => m.Description, friend.Description)
				.SetProperty(m => m.ImgUrl, friend.ImgUrl)
				.SetProperty(m => m.UpdatedAt, friend.UpdatedAt)
		);

			return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
		})
		.WithName("UpdateFriend");

		group.MapPost("/", async (Friend friend, FriendServiceContext db) =>
		{
			db.Friend.Add(friend);
			await db.SaveChangesAsync();
			return TypedResults.Created($"/api/Friend/{friend.Id}", friend);
		})
		.WithName("CreateFriend");

		group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, FriendServiceContext db) =>
		{
			var affected = await db.Friend
				.Where(model => model.Id == id)
				.ExecuteDeleteAsync();

			return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
		})
		.WithName("DeleteFriend");
	}
}
