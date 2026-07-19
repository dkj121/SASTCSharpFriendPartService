using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SASTCSharpFriendPartService.Hubs;
using SASTCSharpFriendPartService.Models;

namespace SASTCSharpFriendPartService.Endpoints;

public static class SessionEndpoints
{
    public static void MapSessionEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Session").WithTags(nameof(Session));

        group.MapGet("/", async (FriendServiceContext db) =>
        {
            return await db.Session.ToListAsync();
        })
        .WithName("GetAllSessions");

        group.MapGet("/{id}", async Task<Results<Ok<Session>, NotFound>> (int id, FriendServiceContext db) =>
        {
            return await db.Session.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Session model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetSessionById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Session session, FriendServiceContext db) =>
        {
            var affected = await db.Session
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.Title, session.Title)
                .SetProperty(m => m.Content, session.Content)
                .SetProperty(m => m.UpdatedAt, session.UpdatedAt)
        );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateSession");

        group.MapPost("/", async (Session session, FriendServiceContext db, IHubContext<ChatHub> hub) =>
        {
            db.Session.Add(session);
            await db.SaveChangesAsync();
            await hub.Clients.All.SendAsync("SessionCreated", session);
            return TypedResults.Created($"/api/Session/{session.Id}", session);
        })
        .WithName("CreateSession");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, FriendServiceContext db, IHubContext<ChatHub> hub) =>
        {
            var affected = await db.Session
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            if (affected == 1)
            {
                await hub.Clients.All.SendAsync("SessionDeleted", id);
                return TypedResults.Ok();
            }
            return TypedResults.NotFound();
        })
        .WithName("DeleteSession");
    }
}
