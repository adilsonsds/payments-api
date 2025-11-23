namespace Payments.Application.Commands.v1.UpdatePayment;

public record UpdatePaymentCommand(
    int PaymentId,
    string? Content,
    string? Description,
    decimal? Amount,
    DateTime? PaymentDate,
    bool? Completed,
    int? BalanceId
) : ICommand<UpdatePaymentCommandResponse>;
