namespace Payments.Application.Queries.v1.GetPaymentsSummary;

public record GetPaymentsSummaryQueryResponse(IEnumerable<GetPaymentsSummaryQueryResponseItem> Items);
