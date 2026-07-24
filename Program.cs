using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using SASTCSharpFriendPartService.Data;
using SASTCSharpFriendPartService.Endpoints;
using SASTCSharpFriendPartService.Hubs;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FriendServiceContext");

// 使用 SQLite 作为数据库上下文
builder.Services.AddDbContext<FriendServiceContext>(options =>
{
	options.UseSqlite("Data Source=FriendService.db");
});

// 设置 JSON 序列化选项以忽略循环引用
builder.Services.ConfigureHttpJsonOptions(options =>
	options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// CORS + SignalR: 允许来自任何来源的请求，允许任何方法和头部，并允许凭据
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

// 以此顺序映射端点：ChatLogEndpoints、FriendEndpoints、SessionEndpoints、ChatHub
app.MapChatLogEndpoints();

app.MapFriendEndpoints();

app.MapSessionEndpoints();

app.MapHub<ChatHub>("/chatHub");

// 初始化数据库
await DbInitializer.InitializeAsync(app);

app.Run();

