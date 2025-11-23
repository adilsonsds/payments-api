namespace Payments.Application.Queries.v1.GetBalances;

public record GetBalancesQuery : IQuery<GetBalancesQueryResponse>
{
    public int ProfileId { get; init; }
    public int StartMonth { get; init; }
    public int StartYear { get; init; }
    public int EndMonth { get; init; }
    public int EndYear { get; init; }
}
