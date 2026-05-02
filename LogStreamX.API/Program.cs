using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================== CONTROLLERS ==================
builder.Services.AddControllers();

// ================== SWAGGER ==================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================== DB FIX (IMPORTANT) ==================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");

    // FORCE SQL SERVER CORRECTLY
    options.UseSqlServer(conn, sql =>
    {
        sql.EnableRetryOnFailure();
    });
});

// ================== CORS (UI FIX) ==================
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p =>
    {
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod();
    });
});

var app = builder.Build();

// ================== PIPELINE ==================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LogStreamX API v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles(); // IMPORTANT FOR index.html

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// ================== HEALTH ROUTE ==================
app.MapGet("/", () => "LogStreamX API Running 🚀");

app.Run();