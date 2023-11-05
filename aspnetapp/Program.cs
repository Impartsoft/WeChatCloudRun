using aspnetapp;
using SignalRChat.Hubs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<CounterContext>();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();

var app = builder.Build();


//app.Map("/",)

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");

app.Run();
