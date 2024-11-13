using Microsoft.AspNetCore.Mvc;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Security.Authentication;

namespace ProcraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly ProcraftDbContext _context;

        public AuthenticationController(ProcraftDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] ProcraftAuthentication data)
        {
            await _context.Authentication.AddAsync(data);

            await _context.SaveChangesAsync();

            return Created($"{this.HttpContext.Request.Path}", data);
        }

        [HttpGet("/{email}")]
        public async Task<IActionResult> Authenticate(string email)
        {
            var authenticationData = await _context.Authentication.FindAsync(email);

            if (authenticationData == null)
            {
                return NotFound(new
                {
                    Message = "Invalid credentials. Please, verify your data and try again."
                });
            }

            return Ok(authenticationData);
        }

    }
}
