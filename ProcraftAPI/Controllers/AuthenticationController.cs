using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.Security;
using ProcraftAPI.Dtos.User;
using ProcraftAPI.Dtos.User.Address;
using ProcraftAPI.Entities.User;
using ProcraftAPI.Interfaces;
using ProcraftAPI.Security.Authentication;

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

            }
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
            .Include(u => u.Address)
            .FirstOrDefaultAsync();

        var credentialOwner = new UserDto
        {
            Id = userData!.Id,
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
                UserId = userData!.Id
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
            }
        };

        return Ok(credentialOwner);
    }

}
