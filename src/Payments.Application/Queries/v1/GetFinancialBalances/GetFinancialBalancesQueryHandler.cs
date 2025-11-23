using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetFinancialBalances;

public class GetFinancialBalancesQueryHandler(PaymentsDbContext paymentsDbContext) 
    : IQueryHandler<GetFinancialBalancesQuery, GetFinancialBalancesQueryResponse>
{
    private readonly PaymentsDbContext _paymentsDbContext = paymentsDbContext;

    public async Task<GetFinancialBalancesQueryResponse> HandleAsync(GetFinancialBalancesQuery request, CancellationToken cancellationToken)
    {
        var financialBalances = await _paymentsDbContext.FinancialBalances
            .AsNoTracking()
            .Where(pb => pb.Profile.Id == request.ProfileId
                   && (pb.Year > request.StartYear || (pb.Year == request.StartYear && pb.Month >= request.StartMonth))
                         && (pb.Year < request.EndYear
                             || (pb.Year == request.EndYear && pb.Month <= request.EndMonth)))
            .OrderBy(pb => pb.Year)
            .ThenBy(pb => pb.Month)
            .ToListAsync(cancellationToken);

        var agrupedFinancialBalances = financialBalances
            .GroupBy(pb => new { pb.Year, pb.Month })
            .Select(g => new GetFinancialBalancesQueryResponseMonth(
                g.Key.Year,
                g.Key.Month,
                [.. g.Select(pb => new GetFinancialBalancesQueryResponseCategory(
                    pb.Category,
                    pb.Amount
                ))]
            ))
            .ToList();

        return new GetFinancialBalancesQueryResponse(agrupedFinancialBalances);
    }
}