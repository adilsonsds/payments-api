using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetBalances;

public class GetBalancesQueryHandler(PaymentsDbContext paymentsDbContext) 
    : IQueryHandler<GetBalancesQuery, GetBalancesQueryResponse>
{
    private readonly PaymentsDbContext _paymentsDbContext = paymentsDbContext;

    public async Task<GetBalancesQueryResponse> HandleAsync(GetBalancesQuery request, CancellationToken cancellationToken)
    {
        var balances = await _paymentsDbContext.Balances
            .AsNoTracking()
            .Where(pb => pb.Profile.Id == request.ProfileId
                   && (pb.Year > request.StartYear || (pb.Year == request.StartYear && pb.Month >= request.StartMonth))
                         && (pb.Year < request.EndYear
                             || (pb.Year == request.EndYear && pb.Month <= request.EndMonth)))
            .OrderBy(pb => pb.Year)
            .ThenBy(pb => pb.Month)
            .ToListAsync(cancellationToken);

        var groupedBalances = balances
            .GroupBy(pb => new { pb.Year, pb.Month })
            .Select(g => new GetBalancesQueryResponseMonth(
                g.Key.Year,
                g.Key.Month,
                [.. g.Select(pb => new GetBalancesQueryResponseCategory(
                    pb.Description,
                    pb.PlannedAmount
                ))]
            ))
            .ToList();

        return new GetBalancesQueryResponse(groupedBalances);
    }
}