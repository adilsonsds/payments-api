namespace Payments.Application.Queries.v1.GetPlannedBalances;

public record GetPlannedBalancesQueryResponse(
    List<GetPlannedBalancesQueryResponseMonth> Items
);

public record GetPlannedBalancesQueryResponseMonth(
    int Year,
    int Month,
    List<GetPlannedBalancesQueryResponseCategory> Categories);

public record GetPlannedBalancesQueryResponseCategory(string Category, decimal Amount);