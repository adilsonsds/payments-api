namespace Payments.Application.Commands.v1.UpdatePayment;

public record UpdatePaymentCommandRequest(
    string? Content,
    string? Description,
    decimal? Amount,
    DateTime? PaymentDate,
    bool? Completed
);
