using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;
using ProcraftAPI.Dtos.Process.Step.Action;
using ProcraftAPI.Entities.Process.Step;
using ProcraftAPI.Enums;

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

            if (step == null)
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
                StepId = newAction.StepId
            };

            return Created(this.Request.Path, actionDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActionByIdAsync(Guid id)
        {
            var action = await _context.Action.FindAsync(id);

            if (action == null)
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
                StartedAt = action.StartedAt,
                FinishedAt = action.FinishedAt,
                Duration = action.Duration,
                UserId = action.UserId,
                StepId = action.StepId
            };

            return Ok(actionDto);
        }

        [HttpPatch("start")]
        public async Task<IActionResult> StartActionAsync([FromBody] StartActionDto dto)
        {
            var user = await _context.User.FindAsync(dto.userId);

            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"User with id {dto.userId} not found."
                });
            }

            var action = await _context.Action.FindAsync(dto.actionId);

            if (action == null)
            {
                return NotFound(new
                {
                    Message = $"Action with id {dto.actionId} not found."
                });
            }

            bool validUser = action.UserId == dto.userId;

            if (!validUser)
            {
                return BadRequest(new
                {
                    Message = $"This user cannot apply changes to the current action. User id {dto.userId}."
                });
            }

            action.StartedAt = dto.StartedAt;

            action.Progress = Progress.Started;

            var step = await _context.Step.FindAsync(action.StepId);

            bool nonInitializedStep = step!.Progress == Progress.Created;

            if (nonInitializedStep)
            {
                step.Progress = Progress.Started;
            }

            var process = await _context.Process.FindAsync(step.ProcessId);

            bool nonInitializedProcess = process!.Progress == Progress.Created;

            if (nonInitializedStep)
            {
                process.Progress = Progress.Started;
            }

            await _context.SaveChangesAsync();

            var actionDto = new ActionDto
            {
                Id = action.Id,
                Title = action.Title,
                Description = action.Description,
                Progress = action.Progress,
                StartedAt = action.StartedAt,
                UserId = action.UserId,
                StepId = action.StepId,
                Duration = action.Duration,
            };

            return Ok(actionDto);
        }

        [HttpPatch("finish")]
        public async Task<IActionResult> FinishActionAsync([FromBody] FinishActionDto dto)
        {
            var user = await _context.User.FindAsync(dto.userId);

            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"User with id {dto.userId} not found."
                });
            }

            var action = await _context.Action.FindAsync(dto.actionId);

            if (action == null)
            {
                return NotFound(new
                {
                    Message = $"Action with id {dto.actionId} not found."
                });
            }

            bool validUser = action.UserId == dto.userId;

            if (!validUser)
            {
                return BadRequest(new
                {
                    Message = $"This user cannot apply changes to the current action. User id {dto.userId}."
                });
            }

            action.FinishedAt = dto.FinishedAt;
            action.Progress = Progress.Finished;

            await _context.SaveChangesAsync();

            var startTimeTicks = action.StartedAt.Ticks;

            var finishingTimeTicks = action.FinishedAt.Ticks;

            var timeExpended = (finishingTimeTicks - startTimeTicks);

            var actionDuration = DateTime.FromBinary(timeExpended);

            var actionDto = new ActionDto
            {
                Id = action.Id,
                Title = action.Title,
                Description = action.Description,
                Progress = action.Progress,
                StartedAt = action.StartedAt,
                FinishedAt = action.FinishedAt,
                Duration = actionDuration,
                UserId = action.UserId,
                StepId = action.StepId,
            };

            return Ok(actionDto);
        }

        [HttpPatch("{id}/user/{userId}")]
        public async Task<IActionResult> UpdateActionAsync(Guid id, Guid userId, [FromBody] UpdateActionDto dto)
        {
            var user = await _context.User.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"User with id {userId} not found."
                });
            }

            var action = await _context.Action.FindAsync(id);

            if (action == null)
            {
                return NotFound(new
                {
                    Message = $"Action with id {id} not found."
                });
            }

            bool validUser = action.UserId == userId;

            if (!validUser)
            {
                return BadRequest(new
                {
                    Message = $"This user cannot apply changes to the current action. User id {userId}."
                });
            }

            action.Title = dto.Title;
            action.Description = dto.Description;
            action.Progress = dto.Progress;

            await _context.SaveChangesAsync();

            var actionDto = new ActionDto
            {
                Id = action.Id,
                Title = action.Title,
                Description = action.Description,
                Progress = action.Progress,
                UserId = action.UserId,
                StepId = action.StepId,
                Duration = action.Duration
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
