namespace Payments.Application.Commands.v1.UpdatePayment;

public record UpdatePaymentCommand(
    int PaymentId,
    int ProfileId,
    string? Content,
    string? Description,
    decimal? Amount,
    DateTime? PaymentDate,
    bool? Completed
) : ICommand;
