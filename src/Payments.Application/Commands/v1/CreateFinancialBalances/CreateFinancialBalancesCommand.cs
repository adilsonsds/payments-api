namespace Payments.Application.Commands.v1.CreateFinancialBalances;

public record CreateFinancialBalancesCommand : ICommand<CreateFinancialBalancesCommandResponse>
{
    public int ProfileId { get; init; }
    public int Year { get; init; }
    public int Month { get; init; }
    public CreateFinancialBalancesCommandCategory[] Categories { get; init; } = null!;
}

public record CreateFinancialBalancesCommandCategory(
    string Category,
    decimal Amount);
