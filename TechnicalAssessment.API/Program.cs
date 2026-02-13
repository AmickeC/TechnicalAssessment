using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.Data;
using TechnicalAssessment.Models;
using TechnicalAssessment.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen();

// Adds all controllers in controllers directory to routes
builder.Services.AddControllers();

// lowercases routes for cleaner routes, e.g. api/Users -> api/users
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

// // Get all users
// app.MapGet("/users", async (AppDbContext db) =>
// {
//     var users = await db.Users
//         .Include(u => u.Groups)
//             .ThenInclude(g => g.Permissions)
//         .ToListAsync();

//     var userDtos = users.Select(u => new UserDto
//     {
//         Id = u.Id,
//         Name = u.Name,
//         Email = u.Email,
//         Groups = u.Groups.Select(g => new GroupDto
//         {
//             Id = g.Id,
//             Name = g.Name,
//             Permissions = g.Permissions.Select(p => new PermissionDto
//             {
//                 Id = p.Id,
//                 Name = p.Name
//             }).ToList()
//         }).ToList()
//     }).ToList();

//     return Results.Ok(userDtos);
// });

// // Get user by ID
// app.MapGet("/users/{id}", async (int id, AppDbContext db) =>
// {
//     var user = await db.Users
//         .Include(u => u.Groups)
//             .ThenInclude(g => g.Permissions)
//         .FirstOrDefaultAsync(u => u.Id == id);

//     if (user is null) return Results.NotFound();

//     var userDto = new UserDto
//     {
//         Id = user.Id,
//         Name = user.Name,
//         Email = user.Email,
//         Groups = user.Groups.Select(g => new GroupDto
//         {
//             Id = g.Id,
//             Name = g.Name,
//             Permissions = g.Permissions.Select(p => new PermissionDto
//             {
//                 Id = p.Id,
//                 Name = p.Name
//             }).ToList()
//         }).ToList()
//     };

//     return Results.Ok(userDto);
// });

// // Add user
// app.MapPost("/users", async (UserCreateDto input, AppDbContext db) =>
// {
//     var groups = await db.Groups
//                          .Where(g => input.GroupIds.Contains(g.Id))
//                          .ToListAsync();
//     var user = new User
//     {
//         Name = input.Name,
//         Email = input.Email,
//         Groups = groups
//     };

//     db.Users.Add(user);
//     await db.SaveChangesAsync();

//     var result = new UserDto
//     {
//         Id = user.Id,
//         Name = user.Name,
//         Email = user.Email,
//         Groups = groups.Select(g => new GroupDto
//         {
//             Id = g.Id,
//             Name = g.Name,
//             Permissions = g.Permissions.Select(p => new PermissionDto
//             {
//                 Id = p.Id,
//                 Name = p.Name
//             }).ToList()
//         }).ToList()
//     };

//     return Results.Created($"/users/{user.Id}", result);
// });


// // Update user
// app.MapPut("/users/{id}", async (int id, UserUpdateDto input, AppDbContext db) =>
// {
//     var user = await db.Users
//                        .Include(u => u.Groups)
//                        .FirstOrDefaultAsync(u => u.Id == id);

//     if (user is null) return Results.NotFound();

//     if (!string.IsNullOrWhiteSpace(input.Name))
//         user.Name = input.Name;

//     if (!string.IsNullOrWhiteSpace(input.Email))
//         user.Email = input.Email;

//     if (input.GroupIds != null && input.GroupIds.Any())
//     {
//         var groups = await db.Groups
//                              .Where(g => input.GroupIds.Contains(g.Id))
//                              .ToListAsync();
//         user.Groups = groups;
//     }

//     await db.SaveChangesAsync();

//     var userDto = new UserDto
//     {
//         Id = user.Id,
//         Name = user.Name,
//         Email = user.Email,
//         Groups = user.Groups.Select(g => new GroupDto
//         {
//             Id = g.Id,
//             Name = g.Name,
//             Permissions = g.Permissions.Select(p => new PermissionDto
//             {
//                 Id = p.Id,
//                 Name = p.Name
//             }).ToList()
//         }).ToList()
//     };

//     return Results.Ok(userDto);
// });

// // Delete user
// app.MapDelete("/users/{id}", async (int id, AppDbContext db) =>
// {
//     var user = await db.Users.FindAsync(id);
//     if (user is null) return Results.NotFound();

//     db.Users.Remove(user);
//     await db.SaveChangesAsync();
//     return Results.NoContent();
// }).WithName("DeleteUser");

// // Total user count
// app.MapGet("/users/count", async (AppDbContext db) =>
//     await db.Users.CountAsync())
//     .WithName("TotalUserCount");

// // Users per group
// app.MapGet("/groups/{groupId}/users/count", async (int groupId, AppDbContext db) =>
// {
//     var count = await db.Groups
//                         .Where(g => g.Id == groupId)
//                         .SelectMany(g => g.Users)
//                         .CountAsync();
//     return Results.Ok(count);
// }).WithName("UsersPerGroup");


// Map the routes added from controllers to make them accessible
app.MapControllers();

app.Run();

