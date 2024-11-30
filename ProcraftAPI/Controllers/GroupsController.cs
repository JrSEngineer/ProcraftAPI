using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.User;
using ProcraftAPI.Entities.User;

namespace ProcraftAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : ControllerBase
{

    private readonly ProcraftDbContext _context;
    public GroupsController(ProcraftDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [ProducesResponseType(typeof(GroupDto), 201)]
    [ProducesErrorResponseType(type: typeof(string))]
    [Produces("application/json")]
    public async Task<IActionResult> CreateGroupAsync([FromBody] NewGroupDto dto)
    {
        var groupId = Guid.NewGuid();

        var newGroup = new ProcraftGroup
        {
            Id = groupId,
            Name = dto.Name
        };

        await _context.Group.AddAsync(newGroup);

        await _context.SaveChangesAsync();

        var newGroupDto = new GroupDto
        {
            Id = newGroup.Id,
            Name = newGroup.Name,
        };

        return Created($"{this.HttpContext.Request.Path}", newGroupDto);
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetGroupsAsync()
    {
        var Groups = await _context.Group
                        .AsNoTracking()
                        .ToListAsync();

        return Ok(Groups);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroupByIdAsync(Guid id)
    {
        var group = await _context.Group
            .AsNoTracking()
            .Where(g => g.Id == id)
            .Include(g => g.Members)
            .ThenInclude(u => u.Authentication)
            .FirstOrDefaultAsync();

        if (group == null)
        {
            return NotFound(new
            {
                Message = $"Group with id {id} not found."
            });
        }

        var groupDto = new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Members = group.Members.Select(u => new UserListDto
            {
                Id = u.Id,
                FullName = u.FullName,
                ProfileImage = u.ProfileImage,
                Description = u.Description,
                PhoneNumber = u.PhoneNumber,
                Cpf = u.Cpf,
                Email = u.Authentication.Email,
                GroupId = u.GroupId
            }).ToList()
        };

        return Ok(groupDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        var group = await _context.Group.FindAsync(id);

        if (group == null)
        {
            return NotFound(new
            {
                Message = $"Group with id {id} not found."
            });
        }

        _context.Group.Remove(group);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
