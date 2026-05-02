using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (IMPORTANT for UI)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

// IMPORTANT: serve static UI (index.html in wwwroot)
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();