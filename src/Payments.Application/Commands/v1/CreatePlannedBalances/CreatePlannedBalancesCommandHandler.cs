using Microsoft.EntityFrameworkCore;
using Payments.Domain;
using Payments.Infra;

namespace Payments.Application.Commands.v1.CreatePlannedBalances;

public class CreatePlannedBalancesCommandHandler(PaymentsDbContext paymentsDbContext)
    : ICommandHandler<CreatePlannedBalancesCommand, CreatePlannedBalancesCommandResponse>
{
    private readonly PaymentsDbContext _paymentsDbContext = paymentsDbContext;

    public async Task<CreatePlannedBalancesCommandResponse> HandleAsync(CreatePlannedBalancesCommand request, CancellationToken cancellationToken)
    {
        await _paymentsDbContext.PlannedBalances
            .Where(pb => pb.Profile.Id == request.ProfileId && pb.Year == request.Year && pb.Month == request.Month)
            .ExecuteDeleteAsync(cancellationToken);

        var profile = await _paymentsDbContext.Profiles
            .FirstOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

        if (profile is null)
        {
            throw new InvalidOperationException($"Profile with Id {request.ProfileId} not found.");
        }

        var newPlannedBalances = request.Categories
            .Select(c => new PlannedBalance
            {
                Profile = profile!,
                Year = request.Year,
                Month = request.Month,
                Category = c.Category,
                Amount = c.Amount
            });

        await _paymentsDbContext.PlannedBalances.AddRangeAsync(newPlannedBalances, cancellationToken);
        await _paymentsDbContext.SaveChangesAsync(cancellationToken);

        return new CreatePlannedBalancesCommandResponse(newPlannedBalances.Sum(pb => pb.Amount));
    }
}