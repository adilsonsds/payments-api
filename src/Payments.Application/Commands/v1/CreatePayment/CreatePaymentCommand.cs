namespace Payments.Application.Commands.v1.CreatePayment;

public record CreatePaymentCommand(
    int ProfileId,
    string Content,
    string? Description,
    decimal Amount,
    DateTime PaymentDate,
    bool Completed,
    int? BalanceId
) : ICommand<CreatePaymentCommandResponse>;
