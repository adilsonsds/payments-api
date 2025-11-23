using Microsoft.EntityFrameworkCore;
using Payments.Domain;
using Payments.Infra;

namespace Payments.Application.Commands.v1.CreateBalances;

public class CreateBalancesCommandHandler(PaymentsDbContext paymentsDbContext)
    : ICommandHandler<CreateBalancesCommand, CreateBalancesCommandResponse>
{
    private readonly PaymentsDbContext _paymentsDbContext = paymentsDbContext;

    public async Task<CreateBalancesCommandResponse> HandleAsync(CreateBalancesCommand request, CancellationToken cancellationToken)
    {
        await _paymentsDbContext.Balances
            .Where(pb => pb.Profile.Id == request.ProfileId && pb.Year == request.Year && pb.Month == request.Month)
            .ExecuteDeleteAsync(cancellationToken);

        var profile = await _paymentsDbContext.Profiles
            .FirstOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

        if (profile is null)
        {
            throw new InvalidOperationException($"Profile with Id {request.ProfileId} not found.");
        }

        var newBalances = request.Categories
            .Select(c => new Balance
            {
                Profile = profile!,
                Year = request.Year,
                Month = request.Month,
                Description = c.Description,
                PlannedAmount = c.PlannedAmount,
                IsInbound = c.IsInbound
            });

        await _paymentsDbContext.Balances.AddRangeAsync(newBalances, cancellationToken);
        await _paymentsDbContext.SaveChangesAsync(cancellationToken);

        return new CreateBalancesCommandResponse(newBalances.Sum(pb => pb.PlannedAmount));
    }
}