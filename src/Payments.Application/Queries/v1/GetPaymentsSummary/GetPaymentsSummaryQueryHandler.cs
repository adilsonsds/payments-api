using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetPaymentsSummary;

public class GetPaymentsSummaryQueryHandler(PaymentsDbContext dbContext) : IQueryHandler<GetPaymentsSummaryQuery, GetPaymentsSummaryQueryResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<GetPaymentsSummaryQueryResponse> HandleAsync(GetPaymentsSummaryQuery query, CancellationToken cancellationToken)
    {
        DateTime fromDate = DateTime.SpecifyKind(new(query.StartYear, query.StartMonth, 1), DateTimeKind.Utc);
        DateTime toDate = DateTime.SpecifyKind(new DateTime(query.EndYear, query.EndMonth, 1).AddMonths(1).AddTicks(-1), DateTimeKind.Utc);

        var paymentsQuery = _dbContext.Payments.AsQueryable()
            .Where(p => p.PaymentDate >= fromDate && p.PaymentDate <= toDate);

        if (query.Profiles != null && query.Profiles.Any())
        {
            paymentsQuery = paymentsQuery.Where(p => query.Profiles.Contains(p.Profile.Id));
        }

        var payments = await paymentsQuery
            .AsNoTracking()
            .GroupBy(p => new { p.Profile.Id, p.Profile.Name, p.PaymentDate.Year, p.PaymentDate.Month })
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .Select(g => new GetPaymentsSummaryQueryResponseItem(
                g.Key.Id,
                g.Key.Name,
                g.Key.Year,
                g.Key.Month,
                g.Where(p => p.Amount > 0).Sum(p => p.Amount),
                g.Where(p => p.Amount < 0).Sum(p => p.Amount),
                g.Where(p => p.Amount > 0 && p.Completed).Sum(p => p.Amount),
                g.Where(p => p.Amount < 0 && p.Completed).Sum(p => p.Amount)
            ))
            .ToListAsync(cancellationToken);

        return new GetPaymentsSummaryQueryResponse(payments);
    }
}
