using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FriendServiceContext") ?? throw new InvalidOperationException("Connection string 'FriendServiceContext' not found.");

builder.Services.AddDbContext<FriendServiceContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapChatLogEndpoints();

app.MapFriendEndpoints();

app.MapSessionEndpoints();

app.Run();

