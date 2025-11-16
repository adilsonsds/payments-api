namespace Payments.Application.Commands.v1.CreatePlannedBalances;

public record CreatePlannedBalancesCommand : ICommand<CreatePlannedBalancesCommandResponse>
{
    public int ProfileId { get; init; }
    public int Year { get; init; }
    public int Month { get; init; }
    public CreatePlannedBalancesCommandCategory[] Categories { get; init; } = null!;
}

public record CreatePlannedBalancesCommandCategory(
    string Category,
    decimal Amount);
