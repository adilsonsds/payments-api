using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Commands.v1.UpdatePayment;

public class UpdatePaymentCommandHandler(PaymentsDbContext dbContext) : ICommandHandler<UpdatePaymentCommand, UpdatePaymentCommandResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<UpdatePaymentCommandResponse> HandleAsync(UpdatePaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = await _dbContext.Payments.FindAsync([command.PaymentId], cancellationToken)
            ?? throw new KeyNotFoundException($"Payment with ID {command.PaymentId} not found.");

        if (command.Content is not null)
            payment.Content = command.Content;

        if (command.Description is not null)
            payment.Description = command.Description;

        if (command.Amount is not null)
            payment.Amount = command.Amount.Value;

        if (command.PaymentDate is not null)
            payment.PaymentDate = command.PaymentDate.Value.ToUniversalTime();

        if (command.Completed is not null)
            payment.Completed = command.Completed.Value;

        if (command.BalanceId is not null)
        {
            var balance = await _dbContext.Balances
                .FirstOrDefaultAsync(pb => pb.Profile.Id == payment.Profile.Id && pb.Id == command.BalanceId, cancellationToken);

            if (balance != null)
            {
                payment.Balance = balance;
            }
        }

        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdatePaymentCommandResponse(
            payment.Id,
            payment.Content,
            payment.Description,
            payment.Amount,
            payment.PaymentDate,
            payment.Completed,
            payment.CreatedAt,
            payment.Balance?.Id
        );
    }
}