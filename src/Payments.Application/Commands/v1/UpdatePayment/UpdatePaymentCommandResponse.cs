namespace Payments.Application.Commands.v1.UpdatePayment;

public record UpdatePaymentCommandResponse(
    int Id,
    string Content,
    string? Description,
    decimal Amount,
    DateTime PaymentDate,
    bool Completed,
    DateTime CreatedAt
);