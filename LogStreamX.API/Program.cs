using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Detect Render environment
var isRender = Environment.GetEnvironmentVariable("RENDER") != null;

// DB SWITCH
if (isRender)
{
    builder.Services.AddDbContext<LogDbContext>(options =>
        options.UseInMemoryDatabase("LogStreamXDb"));
}
else
{
    builder.Services.AddDbContext<LogDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();