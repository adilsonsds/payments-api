namespace Payments.Application.Commands.v1.CreatePayment;

public record CreatePaymentCommand(
    int ProfileId,
    string Content,
    string? Description,
    decimal Amount,
    DateTime PaymentDate,
    bool Completed
) : ICommand<CreatePaymentCommandResponse>;

public record CreatePaymentCommandResponse(
    int Id,
    string Content,
    string? Description,
    decimal Amount,
    DateTime PaymentDate,
    bool Completed,
    DateTime CreatedAt
);