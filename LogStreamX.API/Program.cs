using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================= CONTROLLERS =================
builder.Services.AddControllers();

// ================= SWAGGER =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "LogStreamX API",
        Version = "v1",
        Description = "Production Logging System"
    });

    // Prevent schema crash
    options.CustomSchemaIds(type => type.FullName);
});

// ================= DB =================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(conn);
});

// ================= CORS =================
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

// ================= PIPELINE ORDER (IMPORTANT) =================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LogStreamX API v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

// Health check
app.MapGet("/", () => "LogStreamX API Running 🚀");

app.Run();