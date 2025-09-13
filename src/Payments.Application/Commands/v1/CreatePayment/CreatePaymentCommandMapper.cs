namespace Payments.Application.Commands.v1.CreatePayment;

public static class CreatePaymentCommandMapper
{
    public static CreatePaymentCommand ToCommand(this CreatePaymentCommandRequest request, int profileId) =>
        new(
            ProfileId: profileId,
            Content: request.Content,
            Description: request.Description,
            Amount: request.Amount,
            PaymentDate: request.PaymentDate,
            Completed: request.Completed
        );
}