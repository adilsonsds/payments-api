using Payments.Domain;
using Payments.Infra;

namespace Payments.Application.Commands.v1.CreatePayment;

public class CreatePaymentCommandHandler(PaymentsDbContext dbContext) : ICommandHandler<CreatePaymentCommand>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task HandleAsync(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var profile = await _dbContext.Profiles.FindAsync([command.ProfileId], cancellationToken)
            ?? throw new Exception($"Profile with ID {command.ProfileId} not found.");

        var payment = new Payment
        {
            Content = command.Content,
            Description = command.Description,
            Amount = command.Amount,
            PaymentDate = command.PaymentDate,
            Completed = command.Completed,
            Profile = profile,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}