using Microsoft.EntityFrameworkCore;
using Payments.Domain;
using Payments.Infra;

namespace Payments.Application.Commands.v1.CreatePayment;

public class CreatePaymentCommandHandler(PaymentsDbContext dbContext) : ICommandHandler<CreatePaymentCommand, CreatePaymentCommandResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<CreatePaymentCommandResponse> HandleAsync(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var profile = await _dbContext.Profiles.FindAsync([command.ProfileId], cancellationToken)
            ?? throw new Exception($"Profile with ID {command.ProfileId} not found.");

        var payment = new Payment
        {
            Content = command.Content,
            Description = command.Description,
            Amount = command.Amount,
            PaymentDate = command.PaymentDate.ToUniversalTime(),
            Completed = command.Completed,
            Profile = profile,
            CreatedAt = DateTime.UtcNow
        };

        if (command.BalanceId is not null)
        {
            var balance = await _dbContext.Balances
                .FirstOrDefaultAsync(pb => pb.Profile.Id == payment.Profile.Id && pb.Id == command.BalanceId, cancellationToken);

            if (balance != null)
            {
                payment.Balance = balance;
            }
        }

        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreatePaymentCommandResponse(
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