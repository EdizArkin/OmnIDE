using Microsoft.EntityFrameworkCore;
using OmnIDEApi.Data;
using OmnIDEApi.Repositories;
using OmnIDEApi.Models;  // Project sınıfı için ekleyin

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowElectron", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "app://.")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProjectConfigurationRepository, ProjectConfigurationRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ILanguageConfigRepository, LanguageConfigRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();

var app = builder.Build();

// Database creation and migration
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var context = services.GetRequiredService<AppDbContext>();
        
        // Check if database exists and has pending migrations
        if (!context.Database.CanConnect() || context.Database.GetPendingMigrations().Any())
        {
            // Apply any pending migrations
            context.Database.Migrate();
            
            // Only seed if this is a new database (no projects exist)
            if (!context.Projects.Any())
            {
                var testProject = new Project
                {
                    Name = "Test Project",
                    Description = "This is a test project",
                    CreatedDate = DateTime.UtcNow,
                    Language = "C#",
                    Status = "Active"
                };
                context.Projects.Add(testProject);
                await context.SaveChangesAsync();
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database migration/initialization error occurred.");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowElectron");
app.UseAuthorization();
app.MapControllers();

app.Run();
