namespace Payments.Application.Queries.v1.GetPaymentsSummary;

public record GetPaymentsSummaryQuery() : IQuery<GetPaymentsSummaryQueryResponse>
{
    public int[] Profiles { get; init; } = [];
    public int StartYear { get; init; } = DateTime.UtcNow.AddMonths(-1).Year;
    public int StartMonth { get; init; } = DateTime.UtcNow.AddMonths(-1).Month;
    public int EndYear { get; init; } = DateTime.UtcNow.AddMonths(10).Year;
    public int EndMonth { get; init; } = DateTime.UtcNow.AddMonths(10).Month;
}
