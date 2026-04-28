var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () => Results.Redirect("/index.html"));

// 🔥 changed port to avoid conflict
app.Run("http://localhost:7200");