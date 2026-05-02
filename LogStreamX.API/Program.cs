using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================= CONTROLLERS =================
builder.Services.AddControllers();

// ================= DB CONTEXT =================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");

    // 🔥 SAFETY CHECK (prevents hidden null/invalid config crashes)
    if (string.IsNullOrEmpty(conn))
        throw new Exception("DB Connection String is missing");

    options.UseSqlServer(conn);
});

// ================= CORS =================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
    {
        p.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

// ================= SWAGGER =================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "LogStreamX API",
        Version = "v1"
    });

    // 🔥 prevents Swagger crash
    c.CustomSchemaIds(x => x.FullName);
});

var app = builder.Build();

// ================= PIPELINE =================
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// HEALTH CHECK
app.MapGet("/", () => "LogStreamX API Running 🚀");

app.Run();