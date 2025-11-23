using Microsoft.EntityFrameworkCore;
using Payments.Domain;
using Payments.Infra;

namespace Payments.Application.Commands.v1.CreateFinancialBalances;

public class CreateFinancialBalancesCommandHandler(PaymentsDbContext paymentsDbContext)
    : ICommandHandler<CreateFinancialBalancesCommand, CreateFinancialBalancesCommandResponse>
{
    private readonly PaymentsDbContext _paymentsDbContext = paymentsDbContext;

    public async Task<CreateFinancialBalancesCommandResponse> HandleAsync(CreateFinancialBalancesCommand request, CancellationToken cancellationToken)
    {
        await _paymentsDbContext.FinancialBalances
            .Where(pb => pb.Profile.Id == request.ProfileId && pb.Year == request.Year && pb.Month == request.Month)
            .ExecuteDeleteAsync(cancellationToken);

        var profile = await _paymentsDbContext.Profiles
            .FirstOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

        if (profile is null)
        {
            throw new InvalidOperationException($"Profile with Id {request.ProfileId} not found.");
        }

        var newFinancialBalances = request.Categories
            .Select(c => new FinancialBalance
            {
                Profile = profile!,
                Year = request.Year,
                Month = request.Month,
                Category = c.Category,
                Amount = c.Amount
            });

        await _paymentsDbContext.FinancialBalances.AddRangeAsync(newFinancialBalances, cancellationToken);
        await _paymentsDbContext.SaveChangesAsync(cancellationToken);

        return new CreateFinancialBalancesCommandResponse(newFinancialBalances.Sum(pb => pb.Amount));
    }
}