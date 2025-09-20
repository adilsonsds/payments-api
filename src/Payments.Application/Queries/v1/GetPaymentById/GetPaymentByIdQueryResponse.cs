namespace Payments.Application.Queries.v1.GetPaymentById;

public record GetPaymentByIdQueryResponse(
    int Id,
    string Content,
    string? Description,
    DateTime PaymentDate,
    decimal Amount,
    bool Completed,
    DateTime CreatedAt
);
