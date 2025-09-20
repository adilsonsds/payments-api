using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Commands.v1.DeletePayment;

public class DeletePaymentCommandHandler(PaymentsDbContext dbContext) : ICommandHandler<DeletePaymentCommand, DeletePaymentCommandResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<DeletePaymentCommandResponse> HandleAsync(DeletePaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = await _dbContext.Payments
            .FirstOrDefaultAsync(p => p.Id == command.PaymentId && p.Profile.Id == command.ProfileId, cancellationToken)
            ?? throw new KeyNotFoundException("Payment not found.");

        var response = new DeletePaymentCommandResponse(
            payment.Id,
            payment.Content,
            payment.Description,
            payment.Amount,
            payment.PaymentDate,
            payment.Completed,
            payment.CreatedAt
        );

        _dbContext.Payments.Remove(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}