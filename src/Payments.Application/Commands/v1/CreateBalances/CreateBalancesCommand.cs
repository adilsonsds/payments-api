namespace Payments.Application.Commands.v1.CreateBalances;

public record CreateBalancesCommand : ICommand<CreateBalancesCommandResponse>
{
    public int ProfileId { get; init; }
    public int Year { get; init; }
    public int Month { get; init; }
    public CreateBalancesCommandCategory[] Categories { get; init; } = null!;
}

public record CreateBalancesCommandCategory(
    string Description,
    decimal PlannedAmount,
    bool IsInbound);
