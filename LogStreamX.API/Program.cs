using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// ⭐ STATIC UI FIX (IMPORTANT)
app.UseDefaultFiles();   // auto loads index.html
app.UseStaticFiles();    // enables wwwroot

// Root redirect (optional but clean)
app.MapGet("/", ctx =>
{
    ctx.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

app.MapControllers();

app.Run();