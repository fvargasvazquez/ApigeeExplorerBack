using ApigeeExplorer.ApiV2.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddJsonConfiguration();
builder.Services.AddCorsConfiguration();
builder.Services.AddApplicationServices();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigins");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Startup logging
// var logger = app.Services.GetRequiredService<ILogger<Program>>();
// logger.LogInformation("🚀 Apigee Explorer API V2 (.NET) starting up...");
// logger.LogInformation("📊 Health check: http://localhost:5001/api/health");
// logger.LogInformation("🔍 Search API: http://localhost:5001/api/search");
// logger.LogInformation("📚 OpenAPI: http://localhost:5001/openapi/v1.json");

app.Run();