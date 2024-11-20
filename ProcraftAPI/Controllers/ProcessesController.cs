using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.Process;
using ProcraftAPI.Dtos.Process.Ability;
using ProcraftAPI.Dtos.Process.Scope;
using ProcraftAPI.Dtos.Process.Step;
using ProcraftAPI.Dtos.User;
using ProcraftAPI.Entities.Process;
using ProcraftAPI.Entities.Process.Scope;
using ProcraftAPI.Entities.Process.Step;

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

        ProcessScope? scope = null;

        ScopeDto? scopeDto = null;

        if (dto.Scope != null)
        {
            Guid scopedId = Guid.NewGuid();

            scope = new ProcessScope
            {
                Id = scopedId,
                Abilities = dto.Scope.Abilities.Select(a => new ScopeAbility
                {
                    Id = Guid.NewGuid(),
                    Name = a.Name,
                    Description = a.Description,
                    ScopeId = scopedId,
                }).ToList()
            };

            scopeDto = new ScopeDto
            {
                Id = scope.Id,
                Abilities = scope.Abilities.Select(a => new ScopeAbilityDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    ScpoeId = a.ScopeId,
                }).ToList()
            };
        }

        var processData = new ProcraftProcess
        {
            Id = processId,
            Title = dto.Title,
            Description = dto.Description,
            Scope = scope
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

        foreach (var step in dto.Steps)
        {
            Guid stepId = Guid.NewGuid();

            var processStep = new ProcessStep
            {
                Id = stepId,
                Title = step.Title,
                Description = step.Description,
                StartForecast = step.StartForecast,
                FinishForecast = step.FinishForecast,
                ProcessId = processId
            };

            processData.Steps.Add(processStep);
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
            Scope = scopeDto,
            Steps = processData.Steps.Select(s => new StepDto
            {
                Id = s.Id,
                Title= s.Title,
                Description = s.Description,
                StartForecast = s.StartForecast,
                FinishForecast = s.FinishForecast, 
                ProcessId = processId
            }).ToList(),
        };

        return Created($"{this.HttpContext.Request.Path}", newProcessDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetProcessesAsync()
    {
        var processes = await _context.Process
                        .AsNoTracking()
                        .ToListAsync();

        return Ok(processes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProcessByIdAsync(Guid id)
    {
        var process = await _context.Process
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Include(p => p.Users)
            .Include(p => p.Scope)
            .Include(p => p.Steps)
            .FirstOrDefaultAsync();

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
