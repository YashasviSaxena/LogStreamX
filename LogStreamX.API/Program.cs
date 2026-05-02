var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for UI
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

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// IMPORTANT: enable static files (THIS FIXES UI)
app.UseDefaultFiles();   // looks for index.html
app.UseStaticFiles();    // serves wwwroot

app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

// FORCE ROOT ROUTE → index.html (CRITICAL FIX)
app.MapGet("/", context =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

// Render port fix
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");