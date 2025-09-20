namespace Payments.Application.Commands.v1.DeletePayment;

public record DeletePaymentCommand(int PaymentId, int ProfileId) : ICommand<DeletePaymentCommandResponse>;
