using Payments.Infra;

namespace Payments.Application.Commands.v1.UpdatePayment;

public class UpdatePaymentCommandHandler(PaymentsDbContext dbContext) : ICommandHandler<UpdatePaymentCommand>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task HandleAsync(UpdatePaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = await _dbContext.Payments.FindAsync([command.PaymentId], cancellationToken)
            ?? throw new KeyNotFoundException($"Payment with ID {command.PaymentId} not found.");

        if (payment.Profile.Id != command.ProfileId)
            throw new UnauthorizedAccessException("You do not have permission to update this payment.");

        if (command.Content is not null)
            payment.Content = command.Content;

        if (command.Description is not null)
            payment.Description = command.Description;

        if (command.Amount is not null)
            payment.Amount = command.Amount.Value;

        if (command.PaymentDate is not null)
            payment.PaymentDate = command.PaymentDate.Value;

        if (command.Completed is not null)
            payment.Completed = command.Completed.Value;

        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}