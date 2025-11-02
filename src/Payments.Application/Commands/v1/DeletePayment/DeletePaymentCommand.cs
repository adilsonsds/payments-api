namespace Payments.Application.Commands.v1.DeletePayment;

public record DeletePaymentCommand(int PaymentId) : ICommand<DeletePaymentCommandResponse>;
