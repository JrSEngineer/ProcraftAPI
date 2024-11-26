using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.Process.Step.Action;
using ProcraftAPI.Entities.Process.Step;

namespace ProcraftAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly ProcraftDbContext _context;

        public ActionsController(ProcraftDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateActionAsync([FromBody] NewProcessActionDto dto)
        {
            var step = await _context.Step
                .Where(s => s.Id == dto.StepId)
                .Include(s => s.Users)
                .FirstOrDefaultAsync();

            if(step == null)
            {
                return NotFound(new
                {
                    Message = $"Step with id {dto.StepId} not found."
                });
            }

            bool allowedUserCreation = step!.Users!.Any(u => u.Id == dto.UserId);

            if (!allowedUserCreation)
            {
                return BadRequest(new
                {
                    Message = $"User with id {dto.UserId} is not envolved in current step."
                });
            }

            var newAction = new ProcessAction
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                UserId = dto.UserId,
                StepId = dto.StepId
            };

            await _context.Action.AddAsync(newAction);
            
            await _context.SaveChangesAsync();

            var actionDto = new ActionDto
            {
                Id = newAction.Id,
                Title = newAction.Title,
                Description = newAction.Description,
                Progress = newAction.Progress,
                UserId = newAction.UserId,
                StepId= newAction.StepId
            };

            return Created(this.Request.Path, actionDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActionByIdAsync(Guid id)
        {
            var action = await _context.Action.FindAsync(id);

            if(action == null)
            {
                return NotFound(new
                {
                    Message = $"Action with id {id} not found."
                });
            }

            var actionDto = new ActionDto
            {
                Id = action.Id,
                Title = action.Title,
                Description = action.Description,
                Progress = action.Progress,
                UserId = action.UserId,
                StepId = action.StepId
            };

            return Ok();
        }
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateActionAsync(Guid id, [FromBody] UpdateActionDto dto)
        {
            var action = await _context.Action.FindAsync(id);

            if(action == null)
            {
                return NotFound(new
                {
                    Message = $"Action with id {id} not found."
                });
            }

            action.Title = dto.Title;
            action.Description = dto.Description;
            action.Duration = dto.Duration;
            action.Progress = dto.Progress;

            await _context.SaveChangesAsync();

            var actionDto = new ActionDto
            {
                Id = action.Id,
                Title = action.Title,
                Description = action.Description,
                Progress = action.Progress,
                UserId = action.UserId,
                StepId = action.StepId
            };

            return Ok(actionDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var action = await _context.Action.FindAsync(id);

            if (action == null)
            {
                return NotFound(new
                {
                    Message = $"Action with id {id} not found."
                });
            }

            _context.Action.Remove(action);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
