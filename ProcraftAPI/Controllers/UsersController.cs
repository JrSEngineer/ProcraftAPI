using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.Security;
using ProcraftAPI.Dtos.User.Address;
using ProcraftAPI.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using ProcraftAPI.Dtos.Process;
using ProcraftAPI.Dtos.Process.Step;
using ProcraftAPI.Dtos.Process.Step.Action;

namespace ProcraftAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class UsersController : ControllerBase
{

    private readonly ProcraftDbContext _context;

    public UsersController(ProcraftDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid id)
    {
        var user = await _context.User
            .Where(u => u.Id == id)
            .Include(u => u.Authentication)
            .Include(u => u.Address)
            .Include(u => u.Processes)
            .Include(u => u.Steps)
            .Include(u => u.Actions)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound(new
            {
                Message = $"User with id {id} not found."
            });
        }

        var userDto = new UserDto
        {
            Id = user!.Id,
            ProfileImage = user.ProfileImage,
            FullName = user.FullName,
            Description = user.Description,
            PhoneNumber = user.PhoneNumber,
            GroupId = user.GroupId,
            Cpf = user.Cpf,
            Authentication = new AuthenticationDto
            {
                Email = user.Authentication.Email,
                Role = user.Authentication.Role,
                AccountStatus = user.Authentication.AccountStatus,
                UserId = user!.Id
            },
            Address = new AddressDto
            {
                Id = user.Address.Id,
                Street = user.Address.Street,
                City = user.Address.City,
                State = user.Address.State,
                ZipCode = user.Address.ZipCode,
                Country = user.Address.Country,
                UserId = user.Id,
            },
            Processes = user.Processes?.Select(p => new ProcessListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Progress = p.Progress
            }).ToList(),
            Steps = user.Steps?.Select(s => new StepListDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Progress = s.Progress,
                StartForecast = s.StartForecast,
                FinishForecast = s.FinishForecast,
                ProcessId = s.Id,
            }).ToList(),
            Actions = user!.Actions!.Select(a => new ActionDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Progress = a.Progress,
                Duration = a.Duration,
                UserId = a.UserId,
                StepId = a.StepId,
            }).ToList(),
        };

        return Ok(userDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserAsync()
    {
        var users = await _context.User.ToListAsync();

        var userDtos = users.Select(u => new UserListDto
        {
            Id = u.Id,
            FullName = u.FullName,
            ProfileImage = u.ProfileImage,
            Description = u.Description,
            PhoneNumber = u.PhoneNumber,
            Cpf = u.Cpf,
        }).ToList();

        return Ok(userDtos);
    }

    [HttpGet("{id}/processes")]
    public async Task<IActionResult> GetUserProcessesAsync(Guid id)
    {
        var user = await _context.User
            .AsNoTracking()
            .Include(u => u.Processes)
            .ThenInclude(p => p.Steps)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound(new
            {
                Message = $"User with id {id} not found."
            });
        }


        var processes = user?.Processes.Select(p => {
            double finishedStepsPorcentage = (double)p.CalculateProocessProgressPorcentage();

            return new ProcessListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Progress = p.Progress,
                FinishedStepsPorcentage = finishedStepsPorcentage,
            };
        }).ToList();

        return Ok(processes);
    }
}
