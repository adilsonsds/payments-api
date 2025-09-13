namespace Payments.Application.Commands.v1.CreatePayment;

public record CreatePaymentCommandRequest(
    string Content,
    string? Description,
    decimal Amount,
    DateTime PaymentDate,
    bool Completed
);
