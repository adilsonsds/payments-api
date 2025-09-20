namespace Payments.Application.Commands.v1.DeletePayment;

public record DeletePaymentCommandResponse(
    int Id,
    string Content,
    string? Description,
    decimal Amount,
    DateTime PaymentDate,
    bool Completed,
    DateTime CreatedAt
);