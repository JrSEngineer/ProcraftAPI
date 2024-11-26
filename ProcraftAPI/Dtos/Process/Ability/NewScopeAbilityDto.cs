using System.ComponentModel;

namespace ProcraftAPI.Dtos.Process.Ability;

public record NewScopeAbilityDto
{
    [DefaultValue("Monitoramento de Atividades")]
    public string Name { get; init; } = string.Empty;

    [DefaultValue("Acompanhar passos, calorias e frequência cardíaca.")]
    public string Description { get; init; } = string.Empty;
}