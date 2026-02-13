using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.Data;
using TechnicalAssessment.Models;
using TechnicalAssessment.DTOs;

namespace TechnicalAssessment.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class GroupsController : ControllerBase
{
    private readonly AppDbContext _context;

    public GroupsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/groups
    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        var groups = await _context.Groups
            .Include(g => g.Permissions)
            .Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Permissions = g.Permissions
                    .Select(p => new PermissionDto { Id = p.Id, Name = p.Name })
                    .ToList()
            })
            .ToListAsync();

        return Ok(groups);
    }

    // GET: api/groups/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroup(int id)
    {
        var group = await _context.Groups
            .Include(g => g.Permissions)
            .Where(g => g.Id == id)
            .Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Permissions = g.Permissions
                    .Select(p => new PermissionDto { Id = p.Id, Name = p.Name })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (group == null) return NotFound();

        return Ok(group);
    }
}
