namespace Payments.Application.Queries.v1.GetPayments;

public record GetPaymentsQuery(
    int? ProfileId,
    DateTime? FromDate,
    DateTime? ToDate,
    int PageNumber = 1,
    int PageSize = 10
) : IQuery<GetPaymentsQueryResponse>;
