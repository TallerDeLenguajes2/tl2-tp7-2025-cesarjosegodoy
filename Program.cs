var builder = WebApplication.CreateBuilder(args);

// --- Agregamos servicios Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Agregamos controladores (si usás controllers) ---
builder.Services.AddControllers();

var app = builder.Build();

// --- Activamos Swagger siempre (no solo en Development) ---
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
    c.RoutePrefix = string.Empty; // Esto hace que Swagger sea la raíz "/"
});

// --- Alternativa: redirigir desde "/" a "/swagger" ---
app.MapGet("/", () => Results.Redirect("/swagger"));

// --- Resto del pipeline ---
app.UseAuthorization();
app.MapControllers();

app.Run();