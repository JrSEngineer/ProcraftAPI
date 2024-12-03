using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.User.Manager;
using ProcraftAPI.Entities.User;
using ProcraftAPI.Security.Enums;

namespace ProcraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly ProcraftDbContext _context;

        public ManagersController(ProcraftDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateManagerAsync([FromBody] NewManagerDto dto)
        {
            var group = await _context.Group
            .Where(g => g.Id == dto.GroupId)
            .Include(g => g.Members)
            .ThenInclude(u => u.Authentication)
            .FirstOrDefaultAsync();

            if (group == null)
            {
                return NotFound(new
                {
                    Message = $"Group with id ${dto.GroupId} not found."
                });
            }

            var user = group.Members.Find(m => m.Id == dto.UserId);

            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"User with id ${dto.UserId} not found."
                });
            }

            if(user.Authentication.Role != UserRole.Admin)
            {
                return BadRequest(new
                {
                    Message = $"User with id ${dto.UserId} has no permission to be a manager."
                });
            }

            Guid managerId = Guid.NewGuid();

            var newManager = new ProcessManager
            {
                Id = managerId,
                GroupId = group.Id,
                Email = user.Authentication.Email,
                ProfileImage = user.ProfileImage,
                UserId = user.Id
            };

            await _context.Manager.AddAsync(newManager);

            await _context.SaveChangesAsync();

            var managerDto = new ManagerDto
            {
                Id = newManager.Id,
                UserId = user.Id,
                Email = newManager.Email,
                ProfileImage = newManager.ProfileImage
            };

            return Created(this.Request.Path, managerDto);
        }


        [HttpGet("from-group/{groupId}")]
        public async Task<IActionResult> GetAllManagersAsync(Guid groupId)
        {
            var group = await _context.Group
             .Where(g => g.Id == groupId)
             .Include(g => g.Managers)
             .FirstOrDefaultAsync();

            if (group == null)
            {
                return NotFound(new
                {
                    Message = $"Group with id ${groupId} not found."
                });
            }

            var managers = group.Managers.Select(m => new ManagerListDto
            {
                Id = m.Id,
                UserId = m.UserId,
                Email = m.Email,
                ProfileImage = m.ProfileImage
            });

            return Ok(managers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetManagerByIdAsync(Guid id)
        {
            var manager = await _context.Manager
                .Where(m => m.Id == id)
                .Include(m => m.Processes)
                .FirstOrDefaultAsync();

            if (manager == null)
            {
                return NotFound(new
                {
                    Message = $"Manager with id ${id} not found."
                });
            }

            var managerDto = new ManagerDto
            {
                Id = manager.Id,
                UserId = manager.UserId,
                Email = manager.Email,
                ProfileImage = manager.ProfileImage
            };

            return Ok(managerDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManagerAsync(Guid id)
        {
            var manager = await _context.Manager.FindAsync(id);

            if (manager == null)
            {
                return NotFound(new
                {
                    Message = $"Manager with id ${id} not found."
                });
            }

            _context.Manager.Remove(manager);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
