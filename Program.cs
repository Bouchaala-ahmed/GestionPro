using GestionProAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuration PORT pour Railway (ESSENTIEL)
builder.WebHost.UseUrls($"http://0.0.0.0:{Environment.GetEnvironmentVariable("PORT") ?? "5000"}");

// 2. Configuration Database avec résilience
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ));

// 3. CORS Sécurisé (Adaptez les origines)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", builder =>
    {
        builder.WithOrigins(
                "http://localhost:4200",  // Dev Angular
                "https://votre-front.netlify.app"  // Production
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// 4. Swagger configuré pour Production
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "GestionPro API", Version = "v1" });
});

var app = builder.Build();

// 5. Gestion des Migrations Automatisée
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}

// 6. Pipeline de Requêtes
app.UseCors("AllowedOrigins");

if (!app.Environment.IsEnvironment("Test")) // Swagger sauf en environnement Test
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestionPro API v1");
        c.RoutePrefix = "api-docs";  // Sécurisé
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// 7. Gestion d'Erreurs Globale (PROD)
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/error");
    app.Logger.LogInformation("Mode Production activé");
}

await app.RunAsync();