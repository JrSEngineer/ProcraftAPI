using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.Process;
using ProcraftAPI.Dtos.Process.Ability;
using ProcraftAPI.Dtos.Process.Scope;
using ProcraftAPI.Dtos.Process.Step;
using ProcraftAPI.Dtos.User;
using ProcraftAPI.Dtos.User.Manager;
using ProcraftAPI.Entities.Process;
using ProcraftAPI.Entities.Process.Scope;
using ProcraftAPI.Entities.Process.Step;
using ProcraftAPI.Entities.User;

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
    [HttpPost("in-group/{groupId}")]
    public async Task<IActionResult> CreateProcessAsync(Guid groupId, [FromBody] NewProcessDto dto)
    {
        var group = await _context.Group
                    .Where(g => g.Id == groupId)
                    .Include(g => g.Members)
                    .ThenInclude(m => m.Authentication)
                    .Include(g => g.Managers)
                    .FirstOrDefaultAsync();

        if (group == null)
        {
            return NotFound(new
            {
                Message = $"Group ith id {groupId} not found."
            });
        }

        Guid processId = Guid.NewGuid();

        var usersList = group.Members.ToList();

        var managersList = group.Managers.ToList();

        var managerForProcessCreation = managersList.Find(m => m.Id == dto.ManagerId);

        if (managerForProcessCreation == null)
        {
            return BadRequest(new
            {
                Message = $"There is no manager registered with id {dto.ManagerId}."
            });
        }

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
            ManagerId = dto.ManagerId,
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
            Manager = new ManagerListDto
            {
                Id = managerForProcessCreation.Id,
                UserId = managerForProcessCreation.UserId,
                ProfileImage = managerForProcessCreation.ProfileImage,
                Email = managerForProcessCreation.Email
            },
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
            Steps = processData.Steps.Select(s => new StepListDto
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
    [HttpPost("from-group/{groupId}/to-process/{processId}")]
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

        var responseMessage = dtoList.Count() > 1
            ? $"{dtoList.Count()} Users added to process {processId}."
            : $"{dtoList.Count()} User added to process {processId}.";

        var response = new
        {
            Message = responseMessage
        };

        return Ok(response);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("from-process/{processId}/user/{userId}")]
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

    [HttpGet("from-group/{groupId}")]
    public async Task<IActionResult> GetProcessesAsync(Guid groupId)
    {
        var processes = await _context.Process
                        .AsNoTracking()
                        .Where(p => p.Users.Any(u => u.GroupId == groupId))
                        .ToListAsync();

        var processesList = processes.Select(process => new ProcessListDto
        {
            Id = process.Id,
            Title = process.Title,
            Description = process.Description,
            Progress = process.Progress,
            FinishedStepsPorcentage = process.CalculateProocessProgressPorcentage()
        }).ToList();

        return Ok(processesList);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProcessByIdAsync(Guid id)
    {
        var process = await _context.Process
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Include(p => p.Manager)
            .Include(p => p.Steps)
            .ThenInclude(s => s.Actions)
            .Include(p => p.Users)
            .ThenInclude(u => u.Authentication)
            .Include(p => p.Scope)
            .ThenInclude(s => s.Abilities)
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
            Manager = new ManagerListDto
            {
                Id = process.Manager.Id,
                UserId = process.Manager.Id,
                Email = process.Manager.Email,
                ProfileImage = process.Manager.ProfileImage,
            },
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
                Email = user.Authentication.Email
            }).ToList(),
            Steps = process.Steps.Select(step => new StepListDto
            {
                Id = step.Id,
                Title = step.Title,
                Description = step.Description,
                Progress = step.Progress,
                StartForecast = step.StartForecast,
                FinishForecast = step.FinishForecast,
                ProcessId = step.ProcessId
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
