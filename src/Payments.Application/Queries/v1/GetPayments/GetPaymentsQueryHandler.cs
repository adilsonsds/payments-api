using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetPayments;

public class GetPaymentsQueryHandler(PaymentsDbContext dbContext) : IQueryHandler<GetPaymentsQuery, GetPaymentsQueryResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<GetPaymentsQueryResponse> HandleAsync(GetPaymentsQuery query, CancellationToken cancellationToken)
    {
        var paymentsQuery = _dbContext.Payments.AsQueryable().Where(p => p.Profile.Id == query.ProfileId);

        if (query.FromDate.HasValue)
        {
            paymentsQuery = paymentsQuery.Where(p => p.PaymentDate >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            paymentsQuery = paymentsQuery.Where(p => p.PaymentDate <= query.ToDate.Value);
        }

        var payments = await paymentsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var responseItems = payments.Select(p => new GetPaymentsQueryResponseItem(
            p.Id,
            p.Content,
            p.Description,
            p.PaymentDate,
            p.Amount,
            p.Completed,
            p.CreatedAt
        )).ToList();

        return new GetPaymentsQueryResponse(responseItems);
    }
}