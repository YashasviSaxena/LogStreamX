using LogStreamX.API.Services;
using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//
// 🔹 CONTROLLERS + SWAGGER
//
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
// 🔹 DATABASE (POSTGRES - RENDER)
//
builder.Services.AddDbContext<LogDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

//
// 🔹 BACKGROUND WORKER (INSIDE API)
//
builder.Services.AddHostedService<LogBackgroundWorker>();

//
// 🔹 CORS (for UI if needed)
//
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

//
// 🔹 SWAGGER (enable in all environments for resume visibility)
//
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//
// 🔹 HEALTH CHECK (VERY IMPORTANT FOR RENDER)
//
app.MapGet("/health", () => Results.Ok(new
{
    status = "LogStreamX Running 🚀",
    time = DateTime.UtcNow
}));

app.Run();