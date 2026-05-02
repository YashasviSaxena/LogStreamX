var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (important for UI)
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

// ALWAYS enable Swagger in all environments (for now)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LogStreamX API V1");
    c.RoutePrefix = "swagger"; // /swagger
});

app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

// Render port binding
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");