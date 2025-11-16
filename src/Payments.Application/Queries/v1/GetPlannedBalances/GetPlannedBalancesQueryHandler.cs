using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetPlannedBalances;

public class GetPlannedBalancesQueryHandler(PaymentsDbContext paymentsDbContext) 
    : IQueryHandler<GetPlannedBalancesQuery, GetPlannedBalancesQueryResponse>
{
    private readonly PaymentsDbContext _paymentsDbContext = paymentsDbContext;

    public async Task<GetPlannedBalancesQueryResponse> HandleAsync(GetPlannedBalancesQuery request, CancellationToken cancellationToken)
    {
        var plannedBalances = await _paymentsDbContext.PlannedBalances
            .AsNoTracking()
            .Where(pb => pb.Profile.Id == request.ProfileId
                   && (pb.Year > request.StartYear || (pb.Year == request.StartYear && pb.Month >= request.StartMonth))
                         && (pb.Year < request.EndYear
                             || (pb.Year == request.EndYear && pb.Month <= request.EndMonth)))
            .OrderBy(pb => pb.Year)
            .ThenBy(pb => pb.Month)
            .ToListAsync(cancellationToken);

        var agrupedPlannedBalances = plannedBalances
            .GroupBy(pb => new { pb.Year, pb.Month })
            .Select(g => new GetPlannedBalancesQueryResponseMonth(
                g.Key.Year,
                g.Key.Month,
                [.. g.Select(pb => new GetPlannedBalancesQueryResponseCategory(
                    pb.Category,
                    pb.Amount
                ))]
            ))
            .ToList();

        return new GetPlannedBalancesQueryResponse(agrupedPlannedBalances);
    }
}