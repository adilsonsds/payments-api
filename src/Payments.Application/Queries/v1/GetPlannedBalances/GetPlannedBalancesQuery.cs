namespace Payments.Application.Queries.v1.GetPlannedBalances;

public record GetPlannedBalancesQuery : IQuery<GetPlannedBalancesQueryResponse>
{
    public int ProfileId { get; init; }
    public int StartMonth { get; init; }
    public int StartYear { get; init; }
    public int EndMonth { get; init; }
    public int EndYear { get; init; }
}
