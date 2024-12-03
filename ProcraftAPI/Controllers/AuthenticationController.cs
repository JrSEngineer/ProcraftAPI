using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.Process;
using ProcraftAPI.Dtos.Process.Step.Action;
using ProcraftAPI.Dtos.Process.Step;
using ProcraftAPI.Dtos.Security;
using ProcraftAPI.Dtos.User;
using ProcraftAPI.Dtos.User.Address;
using ProcraftAPI.Entities.User;
using ProcraftAPI.Interfaces;
using ProcraftAPI.Security.Authentication;
using RestSharp;
using ProcraftAPI.Dtos.Authenticarion;
using RestSharp.Authenticators;
using System.Threading;
using ProcraftAPI.Dtos.User.Manager;

namespace ProcraftAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{

    private readonly ProcraftDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IHashService _hashService;

    public AuthenticationController(
        ProcraftDbContext context,
        ITokenService tokenService,
        IHashService hashService)
    {
        _context = context;
        _tokenService = tokenService;
        _hashService = hashService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] NewUserDto dto)
    {
        Guid userId = Guid.NewGuid();

        var userData = new ProcraftUser
        {
            Id = userId,
            ProfileImage = dto.ProfileImage,
            FullName = dto.FullName,
            Description = dto.Description,
            PhoneNumber = dto.PhoneNumber,
            Cpf = dto.Cpf,
            GroupId = dto.GroupId,
            Authentication = new ProcraftAuthentication
            {
                Email = dto.Authentication.Email,
                Password = _hashService.HashValue(dto.Authentication.Password),
                Role = dto.Authentication.Role,
                AccountStatus = dto.Authentication.AccountStatus,
                UserId = userId,
            },
            Address = new UserAddress
            {
                Id = Guid.NewGuid(),
                AddressNumber = dto.Address.AddressNumber,
                Street = dto.Address.Street,
                City = dto.Address.City,
                State = dto.Address.State,
                ZipCode = dto.Address.ZipCode,
                Country = dto.Address.Country,
                UserId = userId
            }
        };

        await _context.User.AddAsync(userData);

        await _context.SaveChangesAsync();

        string token = _tokenService.GenerateToken(userData.Authentication);

        var createdUserDto = new UserDto
        {
            Id = userData.Id,
            ProfileImage = userData.ProfileImage,
            FullName = userData.FullName,
            Description = userData.Description,
            PhoneNumber = userData.PhoneNumber,
            Cpf = userData.Cpf,
            Authentication = new AuthenticationDto
            {
                Email = userData.Authentication.Email,
                Token = token,
                Role = userData.Authentication.Role,
                AccountStatus = userData.Authentication.AccountStatus,
                UserId = userData.Id
            },
            Address = new AddressDto
            {
                Id = userData.Address.Id,
                Street = userData.Address.Street,
                City = userData.Address.City,
                State = userData.Address.State,
                ZipCode = userData.Address.ZipCode,
                Country = userData.Address.Country,
                UserId = userData.Id,

            },
            Processes = userData.Processes?.Select(p => new ProcessListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Progress = p.Progress
            }).ToList(),
            Steps = userData.Steps?.Select(s => new StepListDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Progress = s.Progress,
                StartForecast = s.StartForecast,
                FinishForecast = s.FinishForecast,
                ProcessId = s.Id,
            }).ToList(),
            Actions = userData.Actions?.Select(a => new ActionDto
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

        return Created($"{this.HttpContext.Request.Path}", createdUserDto);
    }

    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] LoginDto dto)
    {
        var authenticationData = await _context.Authentication.FindAsync(dto.email);

        if (authenticationData == null)
        {
            return NotFound(new
            {
                Message = "Invalid credentials. Please, verify your data and try again."
            });
        }

        bool matchingHashValues = _hashService.CompareValue(dto.password, authenticationData.Password);

        if (!matchingHashValues)
        {
            return Unauthorized();
        }

        string token = _tokenService.GenerateToken(authenticationData);

        var authenticationDto = new AuthenticationDto
        {
            Email = authenticationData.Email,
            Token = token,
            Role = authenticationData.Role,
            AccountStatus = authenticationData.AccountStatus,
            UserId = authenticationData.UserId
        };

        var userData = await _context.User
            .Where(u => u.Id == authenticationData.UserId)
            .Include(u => u.Authentication)
            .Include(u => u.Manager)
            .Include(u => u.Address)
            .Include(u => u.Processes)
            .Include(u => u.Steps)
            .Include(u => u.Actions)
            .FirstOrDefaultAsync();

        var credentialOwner = new UserDto
        {
            Id = userData!.Id,
            ProfileImage = userData.ProfileImage,
            FullName = userData.FullName,
            Description = userData.Description,
            PhoneNumber = userData.PhoneNumber,
            Cpf = userData.Cpf,
            GroupId = userData.GroupId,
            Authentication = new AuthenticationDto
            {
                Email = userData.Authentication.Email,
                Token = token,
                Role = userData.Authentication.Role,
                AccountStatus = userData.Authentication.AccountStatus,
                UserId = userData!.Id
            },
            Manager = new ManagerListDto
            {
                Id = userData.Manager.Id,
                UserId = userData.Id,
                Email = userData.Authentication.Email,
                ProfileImage = userData.ProfileImage,
            },
            Address = new AddressDto
            {
                Id = userData.Address.Id,
                Street = userData.Address.Street,
                City = userData.Address.City,
                State = userData.Address.State,
                ZipCode = userData.Address.ZipCode,
                Country = userData.Address.Country,
                UserId = userData.Id,
            },
            Processes = userData.Processes?.Select(p => new ProcessListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Progress = p.Progress
            }).ToList(),
            Steps = userData.Steps?.Select(s => new StepListDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Progress = s.Progress,
                StartForecast = s.StartForecast,
                FinishForecast = s.FinishForecast,
                ProcessId = s.Id,
            }).ToList(),
            Actions = userData!.Actions!.Select(a => new ActionDto
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

        return Ok(credentialOwner);
    }

    [HttpPost("token")]
    public IActionResult VerifyTokenAsync(TokenDto dto)
    {
        bool validToken = _tokenService.ValidateToken(dto.token);

        if (!validToken)
        {
            return Unauthorized();
        }

        return Ok(dto);
    }

    /*  [HttpPost("send-recovery-code")]
      public async Task<IActionResult> UpdatePassword([FromBody] RecoveryCodeEmailDto dto)
      {

          try
          {
              var userAccount = await _context.Authentication.FindAsync(dto.Email);

              if (userAccount == null)
              {
                  return NotFound(new
                  {
                      Message = $"Email {dto.Email} not found."
                  });
              }

              var request = new RestRequest("api/send", method: Method.Post);

              request.AddBody(new
              {

              });


              var response = await _client.PostAsync(request);


              return NoContent();
          }
          catch (Exception)
          {
              return StatusCode(500);
          }
      }*/

}
