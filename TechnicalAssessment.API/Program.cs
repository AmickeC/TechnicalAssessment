using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.Data;
using TechnicalAssessment.Models;
using TechnicalAssessment.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));


// Swagger for API calls - for testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen();

// Adds all controllers in controllers directory to routes
builder.Services.AddControllers();

// lowercases routes for cleaner routes
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Users.Any()) 
    {
        var permRead = new Permission { Name = "Read" };
        var permWrite = new Permission { Name = "Write" };
        db.Permissions.AddRange(permRead, permWrite);

        var groupAdmin = new Group { Name = "Admin", Permissions = new List<Permission> { permRead, permWrite } };
        var groupUser = new Group { Name = "User", Permissions = new List<Permission> { permRead } };
        db.Groups.AddRange(groupAdmin, groupUser);

        var userAlice = new User { Name = "Alice", Email = "alice@example.com", Groups = new List<Group> { groupAdmin } };
        var userBob = new User { Name = "Bob", Email = "bob@example.com", Groups = new List<Group> { groupUser } };
        db.Users.AddRange(userAlice, userBob);

        db.SaveChanges();
    }
}

app.MapControllers();

app.Run();

