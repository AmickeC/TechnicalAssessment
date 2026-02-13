using Microsoft.AspNetCore.Mvc;
using TechnicalAssessment.Data;
using TechnicalAssessment.Models;
using TechnicalAssessment.DTOs;
using Microsoft.EntityFrameworkCore;

namespace TechnicalAssessment.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.Groups)
            .ThenInclude(g => g.Permissions)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Groups = u.Groups.Select(g => new GroupDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Permissions = g.Permissions.Select(p => new PermissionDto
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList()
                }).ToList()
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users
            .Include(u => u.Groups)
            .ThenInclude(g => g.Permissions)
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Groups = u.Groups.Select(g => new GroupDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Permissions = g.Permissions.Select(p => new PermissionDto
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (user == null) return NotFound();

        return Ok(user);
    }
    
      // GET: api/users/count
    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        int count = await _context.Users.CountAsync();
        return Ok(count);
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserCreateDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email
        };

        // Assign groups
        if (dto.GroupIds != null && dto.GroupIds.Any())
        {
            var groups = await _context.Groups
                .Where(g => dto.GroupIds.Contains(g.Id))
                .ToListAsync();

            user.Groups = groups;
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserUpdateDto dto)
    {
        var user = await _context.Users
            .Include(u => u.Groups)
            .ThenInclude(g => g.Permissions)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound();

        if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;

        if (dto.GroupIds != null)
        {
            var groups = await _context.Groups
                .Where(g => dto.GroupIds.Contains(g.Id))
                .ToListAsync();

            user.Groups.Clear();
            foreach (var g in groups)
                user.Groups.Add(g);
        }

        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Groups = user.Groups.Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Permissions = g.Permissions.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList()
            }).ToList()
        };

        return Ok(userDto);
    }
}