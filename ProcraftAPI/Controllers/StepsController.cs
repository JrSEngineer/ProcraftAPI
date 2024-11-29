using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.Process.Step;
using ProcraftAPI.Dtos.Process.Step.Action;
using ProcraftAPI.Dtos.User;
using ProcraftAPI.Entities.Process.Step;

namespace ProcraftAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class StepsController : ControllerBase
{
    private readonly ProcraftDbContext _context;

    public StepsController(ProcraftDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("process/{processId}")]
    public async Task<IActionResult> CreateStepsAsync(Guid processId, [FromBody] List<NewStepDto> dtosList)
    {
        var process = await _context.Process.FindAsync(processId);

        if (process == null)
        {
            return NotFound(new
            {
                Message = $"Process with id {processId} not found."
            });
        }

        foreach (var dto in dtosList)
        {
            var stepId = Guid.NewGuid();

            var newStep = new ProcessStep
            {
                Id = stepId,
                Title = dto.Title,
                Description = dto.Description,
                StartForecast = dto.StartForecast,
                FinishForecast = dto.FinishForecast,
                ProcessId = processId
            };

            _context.Step.Add(newStep);
        }

        await _context.SaveChangesAsync();

        var CreatedSuccessfullyMessage = dtosList.Count() > 1 ? $"{dtosList.Count()} new steps added." : $"{dtosList.Count()} new step added.";

        return Created(this.Request.Path, new { Message = CreatedSuccessfullyMessage });
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinStepAsync([FromBody] JoinStepDto dto)
    {
        var process = await _context.Process
            .Where(p => p.Id == dto.processId)
            .Include(p => p.Users)
            .ThenInclude(u => u.Authentication)
            .Include(p => p.Steps)
            .ThenInclude(s => s.Users)
            .FirstOrDefaultAsync();

        if (process == null)
        {
            return NotFound(new
            {
                Message = $"Process with id {dto.processId} not found."
            });
        }

        var user = process.Users.FirstOrDefault(u => u.Id == dto.userId);

        if (user == null)
        {
            return NotFound(new
            {
                Message = $"User with id {dto.userId} not found in current process."
            });
        }

        var step = process.Steps.Find(s => s.Id == dto.stepId);

        if (step == null)
        {
            return NotFound(new
            {
                Message = $"Step with id {dto.stepId} not found in current process."
            });
        }

        bool userPresentInCurrentStep = step!.Users!.Any(u => u.Id == user.Id);

        if (userPresentInCurrentStep)
        {
            return BadRequest(new
            {
                Message = $"User with id {dto.userId} is already participating from the current step."
            });
        }

        step!.Users!.Add(user);

        await _context.SaveChangesAsync();

        var stepDto = new StepDto
        {
            Id = step.Id,
            Title = step.Title,
            Description = step.Description,
            Progress = step.Progress,
            StartForecast = step.StartForecast,
            FinishForecast = step.FinishForecast,
            ProcessId = step.ProcessId,
            Actions = step!.Actions!.Select(a => new ActionDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Progress = a.Progress,
                Duration = a.Duration,
                UserId = a.UserId,
                StepId = a.StepId
            }).ToList(),
            Users = step!.Users!.Select(u => new UserListDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Description = u.Description,
                ProfileImage = u.ProfileImage,
                PhoneNumber = u.PhoneNumber,
                Cpf = u.Cpf,
                GroupId = u.GroupId,
                Email = u.Authentication.Email               
            }).ToList(),
        };

        return Ok(stepDto);
    }

    [HttpGet("{stepId}")]
    public async Task<IActionResult> GetStepByIdAsync(Guid stepId)
    {
        var step = await _context.Step
            .AsNoTracking()
            .Where(s => s.Id == stepId)
            .Include(s => s.Users)
            .ThenInclude(u => u.Authentication)
            .Include(s => s.Actions)
            .FirstOrDefaultAsync();

        if (step == null)
        {
            return NotFound(new
            {
                Message = $"Step with id {stepId} not found."
            });
        }

        var stepDto = new StepDto
        {
            Id = step.Id,
            Title = step.Title,
            Description = step.Description,
            StartForecast = step.StartForecast,
            FinishForecast = step.FinishForecast,
            Progress = step.Progress,
            ProcessId = step.ProcessId,
            Actions = step.Actions?.Select(a => new ActionDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Duration = a.Duration,
                Progress = a.Progress,
                StepId = a.StepId,
                UserId = a.UserId,
            }).ToList(),
            Users = step.Users?.Select(u => new UserListDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Description = u.Description,
                ProfileImage = u.ProfileImage,
                PhoneNumber = u.PhoneNumber,
                Cpf = u.Cpf,
                Email = u.Authentication.Email,
                GroupId = u.GroupId
            }).ToList(),
        };

        return Ok(stepDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{processId}/step/{stepId}")]
    public async Task<IActionResult> UpdateStepAsync(Guid processId, Guid stepId, UpdateStepDto dto)
    {
        var process = await _context.Process
            .AsNoTracking()
            .Where(p => p.Id == processId)
            .Include(p => p.Steps)
            .FirstOrDefaultAsync();

        if (process == null)
        {
            return NotFound(new
            {
                Message = $"Process with id {processId} not found."
            });
        }

        bool stepFoundInCurrentProcess = process.Steps.Any(s => s.Id == stepId);

        if (!stepFoundInCurrentProcess)
        {
            return NotFound(new
            {
                Message = $"Step with id {stepId} not found in current process."
            });
        }

        var step = await _context.Step.FindAsync(stepId);

        step.Title = dto.Title;
        step.Description = dto.Description;
        step.Progress = dto.Progress;

        await _context.SaveChangesAsync();

        return Ok(process);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStepAsync(Guid id)
    {
        var step = await _context.Step.FindAsync(id);

        if (step == null)
        {
            return BadRequest(new
            {
                Message = $"Step with id {id} not present in current process."
            });
        }

        _context.Step.Remove(step);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
