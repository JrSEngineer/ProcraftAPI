using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.Process;
using ProcraftAPI.Dtos.User;
using ProcraftAPI.Entities.Process;

namespace ProcraftAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ProcessesController : ControllerBase
{

    private readonly ProcraftDbContext _context;
    public ProcessesController(ProcraftDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProcessAsync([FromBody] NewProcessDto dto)
    {
        Guid processId = Guid.NewGuid();

        var processData = new ProcraftProcess
        {
            Id = processId,
            Title = dto.Title,
            Description = dto.Description
        };

        foreach (var user in dto.Users)
        {
            var processUser = await _context.User.FindAsync(user.UserId);

            if (processUser == null)
            {
                return NotFound(new
                {
                    Message = $"User with id {user.UserId} not found."
                });
            }

            processData.Users.Add(processUser);
        }

        await _context.Process.AddAsync(processData);

        await _context.SaveChangesAsync();

        var newProcessDto = new ProcessDto
        {
            Id = processData.Id,
            Title = processData.Title,
            Description = processData.Description,
            Progress = processData.Progress,
            Users = processData.Users.Select(u => new UserListDto
            {
                Id = u.Id,
                FullName = u.FullName,
                ProfileImage = u.ProfileImage,
                Description = u.Description,
                PhoneNumber = u.PhoneNumber,
                Cpf = u.Cpf
            }).ToList(),
        };

        return Created($"{this.HttpContext.Request.Path}", newProcessDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetProcessesAsync()
    {
        var processes = await _context.Process.ToListAsync();

        return Ok(processes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProcessByIdAsync(Guid id)
    {
        var process = await _context.Process.FindAsync(id);

        if (process == null)
        {
            return NotFound(new
            {
                Message = $"Process with id {id} not found."
            });
        }

        return Ok(process);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateProcess(Guid id, UpdateProcessDto dto)
    {
        var process = await _context.Process.FindAsync(id);

        if (process == null)
        {
            return NotFound(new
            {
                Message = $"Process with id {id} not found."
            });
        }

        process.Title = dto.Title;
        process.Description = dto.Description;
        process.Progress = dto.Progress;

        await _context.SaveChangesAsync();

        return Ok(process);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProcess(Guid id)
    {
        var process = await _context.Process.FindAsync(id);

        if (process == null)
        {
            return NotFound(new
            {
                Message = $"Process with id {id} not found."
            });
        }

        _context.Process.Remove(process);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
