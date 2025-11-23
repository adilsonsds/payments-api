namespace Payments.Application.Queries.v1.GetFinancialBalances;

public record GetFinancialBalancesQueryResponse(
    List<GetFinancialBalancesQueryResponseMonth> Items
);

public record GetFinancialBalancesQueryResponseMonth(
    int Year,
    int Month,
    List<GetFinancialBalancesQueryResponseCategory> Categories);

public record GetFinancialBalancesQueryResponseCategory(string Category, decimal Amount);