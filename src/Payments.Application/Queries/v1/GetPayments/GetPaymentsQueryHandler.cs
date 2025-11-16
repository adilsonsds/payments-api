using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetPayments;

public class GetPaymentsQueryHandler(PaymentsDbContext dbContext) : IQueryHandler<GetPaymentsQuery, GetPaymentsQueryResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<GetPaymentsQueryResponse> HandleAsync(GetPaymentsQuery query, CancellationToken cancellationToken)
    {
        var paymentsQuery = _dbContext.Payments.AsQueryable().AsNoTracking();

        if (query.Profiles != null && query.Profiles.Any())
        {
            paymentsQuery = paymentsQuery.Where(p => query.Profiles.Contains(p.Profile.Id));
        }

        if (query.Year.HasValue && query.Month.HasValue)
        {
            int year = query.Year.Value;
            int month = query.Month.Value;
            DateTime fromDate = DateTime.SpecifyKind(new(year, month, 1), DateTimeKind.Utc);
            DateTime toDate = DateTime.SpecifyKind(new DateTime(year, month, 1).AddMonths(1).AddTicks(-1), DateTimeKind.Utc);

            paymentsQuery = paymentsQuery.Where(p => p.PaymentDate >= fromDate && p.PaymentDate <= toDate);
        }

        paymentsQuery = query.SortBy switch
        {
            GetPaymentsQuery.SortByOptions.CreatedAtAsc => paymentsQuery.OrderBy(p => p.CreatedAt),
            GetPaymentsQuery.SortByOptions.CreatedAtDesc => paymentsQuery.OrderByDescending(p => p.CreatedAt),
            GetPaymentsQuery.SortByOptions.PaymentDateAsc => paymentsQuery.OrderBy(p => p.PaymentDate),
            GetPaymentsQuery.SortByOptions.PaymentDateDesc => paymentsQuery.OrderByDescending(p => p.PaymentDate),
            GetPaymentsQuery.SortByOptions.AmountAsc => paymentsQuery.OrderBy(p => p.Amount),
            GetPaymentsQuery.SortByOptions.AmountDesc => paymentsQuery.OrderByDescending(p => p.Amount),
            GetPaymentsQuery.SortByOptions.CompletedAsc => paymentsQuery.OrderBy(p => p.Completed).ThenBy(p => p.PaymentDate),
            GetPaymentsQuery.SortByOptions.CompletedDesc => paymentsQuery.OrderByDescending(p => p.Completed).ThenBy(p => p.PaymentDate),
            _ => paymentsQuery.OrderByDescending(p => p.CreatedAt)
        };

        var payments = await paymentsQuery
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
                p.Profile.Name,
                p.PlannedBalance != null ? p.PlannedBalance.Category : null
            ))
            .ToListAsync(cancellationToken);

        return new GetPaymentsQueryResponse(payments);
    }
}