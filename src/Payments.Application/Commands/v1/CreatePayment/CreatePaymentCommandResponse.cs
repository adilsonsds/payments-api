namespace Payments.Application.Commands.v1.CreatePayment;

public record CreatePaymentCommandResponse(
    int Id,
    string Content,
    string? Description,
    decimal Amount,
    DateTime PaymentDate,
    bool Completed,
    DateTime CreatedAt
);