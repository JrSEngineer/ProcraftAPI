using System.ComponentModel.DataAnnotations;

namespace ProcraftAPI.Dtos.Process;

public record NewProcessDto
{
    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    [Required(ErrorMessage = "Please, provide a valid user id.")]
    public Guid UserId { get; init; }
}
