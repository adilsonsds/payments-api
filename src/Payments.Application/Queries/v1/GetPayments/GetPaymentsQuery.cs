namespace Payments.Application.Queries.v1.GetPayments;

public record GetPaymentsQuery(
    int PageNumber = 1,
    int PageSize = 50
) : IQuery<GetPaymentsQueryResponse>
{
    public int[] Profiles { get; init; } = [];
    public int Year { get; init; } = DateTime.UtcNow.Year;
    public int Month { get; init; } = DateTime.UtcNow.Month;
}
