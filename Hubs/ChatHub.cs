using Microsoft.AspNetCore.SignalR;

namespace SASTCSharpFriendPartService.Hubs;

/// <summary>
/// SignalR hub for real-time chat updates.
/// Desktop clients connect here to receive instant notifications
/// when new posts or replies are created.
/// </summary>
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
