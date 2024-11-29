namespace ProcraftAPI.Dtos.Process.Step;

public record JoinStepDto(Guid processId, Guid stepId, Guid userId);
