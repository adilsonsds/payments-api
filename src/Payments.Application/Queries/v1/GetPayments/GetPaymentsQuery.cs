namespace Payments.Application.Queries.v1.GetPayments;

public record GetPaymentsQuery(
    int PageNumber = 1,
    int PageSize = 50
) : IQuery<GetPaymentsQueryResponse>
{
    public int[] Profiles { get; init; } = [];
    public int? Year { get; init; }
    public int? Month { get; init; }
    public string SortBy { get; set; } = SortByOptions.CreatedAtDesc;

    public static class SortByOptions
    {
        public const string CreatedAtDesc = "CreatedAtDesc";
        public const string CreatedAtAsc = "CreatedAtAsc";
        public const string PaymentDateDesc = "PaymentDateDesc";
        public const string PaymentDateAsc = "PaymentDateAsc";
        public const string AmountDesc = "AmountDesc";
        public const string AmountAsc = "AmountAsc";
        public const string CompletedDesc = "CompletedDesc";
        public const string CompletedAsc = "CompletedAsc";
    }
}
