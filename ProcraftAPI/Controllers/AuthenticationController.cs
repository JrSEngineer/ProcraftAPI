﻿using Microsoft.AspNetCore.Mvc;
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
using ProcraftAPI.Dtos.User.Manager;
using ProcraftAPI.Services;
using System.Net;
using System.Text.Json;

namespace ProcraftAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{

    private readonly ProcraftDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IHashService _hashService;
    private readonly IRestClient _client;
    private readonly EmailTemplateService _templateService;

    public AuthenticationController(
        ProcraftDbContext context,
        ITokenService tokenService,
        IHashService hashService,
        IRestClient client,
        EmailTemplateService templateService)
    {
        _context = context;
        _tokenService = tokenService;
        _hashService = hashService;
        _client = client;
        _templateService = templateService;
    }

    [HttpPost("new-user")]
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

    [HttpPost("login")]
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

        ManagerListDto? managerListDto = null;

        if (userData!.Manager != null)
        {
            managerListDto = new ManagerListDto
            {
                Id = userData!.Manager.Id,
                UserId = userData.Id,
                Email = userData.Authentication.Email,
                ProfileImage = userData.ProfileImage,
            };
        }

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
            Manager = managerListDto,
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

    [HttpPost("send-recovery-code")]
    public async Task<IActionResult> SendRecoveryCode([FromBody] RecoveryCodeEmailDto dto)
    {
        try
        {
            var transactionId = Guid.NewGuid();

            var userAccount = await _context.Authentication
                .Where(a => a.Email == dto.Email)
                .Include(a => a.User)
                .FirstOrDefaultAsync();

            if (userAccount == null)
            {
                return NotFound(new
                {
                    Message = $"Email {dto.Email} not found."
                });
            }

            var accountRecovery = new AccountRecovery
            {
                TransactionId = transactionId,
                RecoveryEmail = userAccount.Email,
                VerificationCode = _hashService.GenerateVerificationCode(6)
            };

            await _context.Recovery.AddAsync(accountRecovery);

            await _context.SaveChangesAsync();

            var request = new RestRequest();

            request.AddHeader("content-type", "application/json");

            var emailContent = _templateService.SetHtmlTemplateValues(userAccount.User.FullName, accountRecovery.VerificationCode);

            var url = _client.Options.BaseUrl;

            var requestBody = JsonSerializer.Serialize(new
            {
                SenderEmailAddress = "procraft.processes.manager@gmail.com",
                ReceiverEmailAddress = userAccount.Email,
                SenderName = "Procraft",
                ReceiverName = userAccount.User.FullName,
                EmailSubject = "Recuperação de Senha",
                EmailContent = emailContent
            });

            request.AddJsonBody(requestBody);

            request.Method = Method.Post;

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(new
                {
                    Message = $"Error sending email to {userAccount.Email}."
                });
            }

            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, $"{e.Message}");
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetAccountPassword([FromBody] RecoveryPasswordDto dto)
    {
        try
        {
            var verificationCode = dto.VerificationCode.ToUpper();

            var recovery = await _context.Recovery
                   .Where(r => r.VerificationCode == verificationCode)
                   .FirstOrDefaultAsync();

            if (recovery == null)
            {
                return NotFound(new
                {
                    Message = "Invalid verification code. Please, verify the sended code and try again."
                });
            };

            bool validPassword = dto.Password == dto.ConfirmationPassword;

            if (!validPassword)
            {
                return BadRequest(new
                {
                    Message = "The passswords has no match. Please, verify the sended passwords and try again."
                });
            }

            var userAccount = await _context.Authentication.FindAsync(recovery.RecoveryEmail);

            userAccount!.Password = _hashService.HashValue(dto.Password);

            await _context.SaveChangesAsync();

            return Ok("Success!");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"{e.Message}");
        }
    }

}
