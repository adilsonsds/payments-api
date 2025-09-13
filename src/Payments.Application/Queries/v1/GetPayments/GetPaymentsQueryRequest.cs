namespace Payments.Application.Queries.v1.GetPayments;

public record GetPaymentsQueryRequest(
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int PageNumber = 1,
    int PageSize = 10
);
