using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===================== CONTROLLERS =====================
builder.Services.AddControllers();

// ===================== SWAGGER =====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===================== DB CONTEXT FIX =====================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrEmpty(connStr))
        throw new Exception("DB connection string missing");

    // 🔥 FORCE SQL SERVER
    options.UseSqlServer(connStr);
});

// ===================== CORS (IMPORTANT FOR UI) =====================
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p =>
    {
        p.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

var app = builder.Build();

// ===================== PIPELINE =====================
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// ===================== ROOT =====================
app.MapGet("/", () => "LogStreamX API Running 🚀");

app.Run();