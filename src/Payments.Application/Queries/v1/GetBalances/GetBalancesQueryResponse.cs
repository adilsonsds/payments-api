namespace Payments.Application.Queries.v1.GetBalances;

public record GetBalancesQueryResponse(
    List<GetBalancesQueryResponseMonth> Items
);

public record GetBalancesQueryResponseMonth(
    int Year,
    int Month,
    List<GetBalancesQueryResponseCategory> Categories);

public record GetBalancesQueryResponseCategory(string Description, decimal PlannedAmount);