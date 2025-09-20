namespace Payments.Application.Queries.v1.GetPayments;

public record GetPaymentsQueryResponseItem(
    int Id,
    string Content,
    string? Description,
    DateTime PaymentDate,
    decimal Amount,
    bool Completed,
    DateTime CreatedAt
);