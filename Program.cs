using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using SASTCSharpFriendPartService.Data;
using SASTCSharpFriendPartService.Endpoints;
using SASTCSharpFriendPartService.Hubs;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FriendServiceContext");

// Use SQL Server if connection string is configured, otherwise fall back to SQLite (easier for Docker/deployment)
builder.Services.AddDbContext<FriendServiceContext>(options =>
{
	if (!string.IsNullOrEmpty(connectionString))
		options.UseSqlServer(connectionString);
	else
		options.UseSqlite("Data Source=FriendService.db");
});

// JSON: ignore navigation cycles to prevent infinite serialization
builder.Services.ConfigureHttpJsonOptions(options =>
	options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// CORS + SignalR: allow desktop clients from any origin
builder.Services.AddCors(options =>
	options.AddDefaultPolicy(policy =>
		policy.SetIsOriginAllowed(_ => true)
			  .AllowAnyMethod()
			  .AllowAnyHeader()
			  .AllowCredentials()));

builder.Services.AddSignalR();

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

app.UseCors();

app.MapChatLogEndpoints();

app.MapFriendEndpoints();

app.MapSessionEndpoints();

app.MapHub<ChatHub>("/chatHub");

// Seed database with initial data
await DbInitializer.InitializeAsync(app);

app.Run();

