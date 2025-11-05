using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetPayments;

public class GetPaymentsQueryHandler(PaymentsDbContext dbContext) : IQueryHandler<GetPaymentsQuery, GetPaymentsQueryResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<GetPaymentsQueryResponse> HandleAsync(GetPaymentsQuery query, CancellationToken cancellationToken)
    {
        DateTime fromDate = DateTime.SpecifyKind(new(query.Year, query.Month, 1), DateTimeKind.Utc);
        DateTime toDate = DateTime.SpecifyKind(new DateTime(query.Year, query.Month, 1).AddMonths(1).AddTicks(-1), DateTimeKind.Utc);

        var paymentsQuery = _dbContext.Payments.AsQueryable()
            .Where(p => p.PaymentDate >= fromDate && p.PaymentDate <= toDate);

        if (query.Profiles != null && query.Profiles.Any())
        {
            paymentsQuery = paymentsQuery.Where(p => query.Profiles.Contains(p.Profile.Id));
        }

        var payments = await paymentsQuery
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new GetPaymentsQueryResponseItem(
                p.Id,
                p.Content,
                p.Description,
                p.PaymentDate,
                p.Amount,
                p.Completed,
                p.CreatedAt,
                p.Profile.Id,
                p.Profile.Name
            ))
            .ToListAsync(cancellationToken);

        return new GetPaymentsQueryResponse(payments);
    }
}