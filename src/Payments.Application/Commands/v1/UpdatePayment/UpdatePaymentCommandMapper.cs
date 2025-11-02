namespace Payments.Application.Commands.v1.UpdatePayment;

public static class UpdatePaymentCommandMapper
{
    public static UpdatePaymentCommand ToCommand(this UpdatePaymentCommandRequest request, int paymentId) =>
        new(
            PaymentId: paymentId,
            Content: request.Content,
            Description: request.Description,
            Amount: request.Amount,
            PaymentDate: request.PaymentDate,
            Completed: request.Completed
        );
}