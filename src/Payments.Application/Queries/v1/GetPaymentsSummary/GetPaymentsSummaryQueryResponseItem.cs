namespace Payments.Application.Queries.v1.GetPaymentsSummary;

public record GetPaymentsSummaryQueryResponseItem(
    int ProfileId,
    string ProfileName,
    int Year,
    int Month,
    decimal TotalAmountIn,
    decimal TotalAmountOut,
    decimal TotalAmountInConfirmed,
    decimal TotalAmountOutConfirmed);
