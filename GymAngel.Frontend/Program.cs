var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Nếu có index.html ở wwwroot thì serve làm trang mặc định
app.UseDefaultFiles();
app.UseStaticFiles();

// Nếu bạn muốn SPA fallback (khi dùng router phía client):
// app.MapFallbackToFile("/index.html");

app.Run();