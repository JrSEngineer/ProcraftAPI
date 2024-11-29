using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.Process;
using ProcraftAPI.Dtos.Process.Ability;
using ProcraftAPI.Dtos.Process.Scope;
using ProcraftAPI.Dtos.Process.Step;
using ProcraftAPI.Dtos.Process.Step.Action;
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

    [Authorize(Roles = "Admin")]
    [HttpPost("{groupId}")]
    public async Task<IActionResult> CreateProcessAsync(Guid groupId, [FromBody] NewProcessDto dto)
    {
        var group = await _context.Group
                    .Where(g => g.Id == groupId)
                    .Include(g => g.Members)
                    .FirstOrDefaultAsync();

        if (group == null)
        {
            return NotFound(new
            {
                Message = $"Group ith id {groupId} not found."
            });
        }

        var usersList = group.Members.ToList();

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

        foreach (var userIdDto in dto.Users)
        {
            var processUser = usersList.Find(u => u.Id == userIdDto.UserId);

            if (processUser == null)
            {
                return NotFound(new
                {
                    Message = $"User with id {userIdDto.UserId} not found in current group."
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
                Title = s.Title,
                Description = s.Description,
                StartForecast = s.StartForecast,
                FinishForecast = s.FinishForecast,
                ProcessId = processId
            }).ToList(),
        };

        return Created($"{this.HttpContext.Request.Path}", newProcessDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{processId}/group/{groupId}/users")]
    public async Task<IActionResult> AddMemberToProcessAsync(Guid processId, Guid groupId, [FromBody] List<UserIdDto> dtoList)
    {
        var group = await _context.Group
           .AsNoTracking()
           .Where(g => g.Id == groupId)
           .Include(g => g.Members)
           .FirstOrDefaultAsync();

        if (group == null)
        {
            return NotFound(new
            {
                Message = $"Group with id {groupId} not found."
            });
        }

        var process = await _context.Process
            .Where(p => p.Id == processId)
            .Include(p => p.Users)
            .ThenInclude(u => u.Authentication)
            .Include(p => p.Scope)
            .Include(p => p.Steps)
            .FirstOrDefaultAsync();

        if (process == null)
        {
            return NotFound(new
            {
                Message = $"Process with id {processId} not found."
            });
        }

        foreach (var dto in dtoList)
        {
            var user = group.Members.Find(u => u.Id == dto.UserId);

            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"User with id {dto.UserId} not found in current group."
                });
            }

            bool userAlreadyPresentInCurrentProcess = process.Users.Any(u => u.Id == user.Id);

            if (userAlreadyPresentInCurrentProcess)
            {
                return BadRequest(new
                {
                    Message = $"User with id {user.Id} already present in current process."
                });
            }

            process!.Users.Add(user);
        }

        await _context.SaveChangesAsync();

        var scopeDto = new ScopeDto
        {
            Id = process!.Scope.Id,
            Abilities = process.Scope.Abilities.Select(a => new ScopeAbilityDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                ScpoeId = a.ScopeId,
            }).ToList()
        };

        var processDto = new ProcessDto
        {
            Id = process.Id,
            Title = process.Title,
            Description = process.Description,
            Progress = process.Progress,
            Users = process.Users.Select(u => new UserListDto
            {
                Id = u.Id,
                FullName = u.FullName,
                ProfileImage = u.ProfileImage,
                Description = u.Description,
                PhoneNumber = u.PhoneNumber,
                Cpf = u.Cpf,
                Email = u.Authentication.Email,
                GroupId = u.GroupId,
            }).ToList(),
            Scope = scopeDto,
            Steps = process.Steps.Select(s => new StepDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                StartForecast = s.StartForecast,
                FinishForecast = s.FinishForecast,
                ProcessId = process.Id
            }).ToList(),
        };

        return Ok(processDto);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{processId}/users/{userId}")]
    public async Task<IActionResult> RemoveMemberFromProcessAsync(Guid processId, Guid userId)
    {
        var process = await _context.Process
           .Where(p => p.Id == processId)
           .Include(p => p.Users)
           .Include(p => p.Scope)
           .Include(p => p.Steps)
           .FirstOrDefaultAsync();

        if (process == null)
        {
            return NotFound(new
            {
                Message = $"Process with id {processId} not found."
            });
        }

        var user = process.Users.Find(u => u.Id == userId);

        if (user == null)
        {
            return NotFound(new
            {
                Message = $"User with id {userId} not found."
            });
        }

        process!.Users.Remove(user);

        await _context.SaveChangesAsync();

        return NoContent();
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
            .ThenInclude(u => u.Authentication)
            .Include(p => p.Users)
            .ThenInclude(u => u.Actions)
            .Include(p => p.Users)
            .ThenInclude(u => u.Steps)
            .Include(p => p.Scope)
            .Include(p => p.Steps)
            .ThenInclude(s => s.Users)
            .Include(p => p.Steps)
            .ThenInclude(s => s.Actions)
            .FirstOrDefaultAsync();

        if (process == null)
        {
            return NotFound(new
            {
                Message = $"Process with id {id} not found."
            });
        }

        var processDto = new ProcessDto
        {
            Id = process.Id,
            Title = process.Title,
            Description = process.Description,
            Progress = process.Progress,
            StartForecast = process.StartForecast,
            FinishForecast = process.FinishForecast,
            StartedAt = process.StartedAt,
            FinishedAt = process.FinishedAt,
            Scope = new ScopeDto
            {
                Id = process.Scope.Id,
                Abilities = process.Scope.Abilities.Select(ability => new ScopeAbilityDto
                {
                    Id = ability.Id,
                    Name = ability.Name,
                    Description = ability.Description,
                    ScpoeId = ability.ScopeId,
                }).ToList(),
            },
            Users = process.Users.Select(user => new UserListDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Description = user.Description,
                ProfileImage = user.ProfileImage,
                PhoneNumber = user.PhoneNumber,
                Cpf = user.Cpf,
                Email = user.Authentication.Email,
                GroupId = user.GroupId,
                Steps = user?.Steps?.Select(step => new StepListDto
                {
                    Id = step.Id,
                    Title = step.Title,
                    Description = step.Description,
                    StartForecast = step.StartForecast,
                    FinishForecast = step.FinishForecast,
                    ProcessId = step.ProcessId,
                    Progress = step.Progress,
                }).ToList() ?? new(),
                Actions = user?.Actions?.Select(action => new ActionDto
                {
                    Id = action.Id,
                    Title = action.Title,
                    Description = action.Description,
                    Progress = action.Progress,
                    Duration = action.Duration,
                    StepId = action.StepId,
                    UserId = action.UserId
                }).ToList() ?? new(),
            }).ToList(),
            Steps = process.Steps.Select(step => new StepDto
            {
                Id = step.Id,
                Title = step.Title,
                Description = step.Description,
                Progress = step.Progress,
                StartForecast = step.StartForecast,
                FinishForecast = step.FinishForecast,
                ProcessId = step.ProcessId,
                Users = step?.Users?.Select(user => new UserListDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Description = user.Description,
                    ProfileImage = user.ProfileImage,
                    PhoneNumber = user.PhoneNumber,
                    Cpf = user.Cpf,
                    Email = user.Authentication.Email,
                    GroupId = user.GroupId,
                }).ToList() ?? new(),
                Actions = step?.Actions?.Select(action => new ActionDto
                {
                    Id = action.Id,
                    Title = action.Title,
                    Description = action.Description,
                    Progress = action.Progress,
                    Duration = action.Duration,
                    StepId = action.StepId,
                    UserId = action.UserId
                }).ToList() ?? new(),
            }).ToList(),

        };

        return Ok(processDto);
    }

    [HttpPatch("{processId}")]
    public async Task<IActionResult> UpdateProcess(Guid processId, UpdateProcessDto dto)
    {
        var process = await _context.Process.FindAsync(processId);

        if (process == null)
        {
            return NotFound(new
            {
                Message = $"Process with id {processId} not found."
            });
        }

        process.Title = dto.Title;
        process.Description = dto.Description;
        process.Progress = dto.Progress;

        await _context.SaveChangesAsync();

        return Ok(process);
    }

    [Authorize(Roles = "Admin")]
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
