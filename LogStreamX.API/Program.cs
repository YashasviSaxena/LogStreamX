using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================= DB =================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ================= CONTROLLERS =================
builder.Services.AddControllers();

// ================= SWAGGER =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================= CORS (FOR UI SAFETY) =================
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

// ================= PIPELINE =================
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseDefaultFiles();   // IMPORTANT (serves index.html)
app.UseStaticFiles();    // IMPORTANT (wwwroot UI)

// ================= CONTROLLERS =================
app.MapControllers();

// ================= HEALTH CHECK =================
app.MapGet("/", () => "LogStreamX API Running 🚀");

app.Run();