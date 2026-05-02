using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ✅ PostgreSQL connection
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();