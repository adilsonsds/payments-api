using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetPaymentById;

public class GetPaymentByIdQueryHandler(PaymentsDbContext dbContext) : IQueryHandler<GetPaymentByIdQuery, GetPaymentByIdQueryResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<GetPaymentByIdQueryResponse> HandleAsync(GetPaymentByIdQuery query, CancellationToken cancellationToken)
    {
        var payment = await _dbContext.Payments
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == query.PaymentId && p.Profile.Id == query.ProfileId, cancellationToken)
            ?? throw new KeyNotFoundException($"Payment with ID {query.PaymentId} not found.");

        return new GetPaymentByIdQueryResponse(
            payment.Id,
            payment.Content,
            payment.Description,
            payment.PaymentDate,
            payment.Amount,
            payment.Completed,
            payment.CreatedAt
        );
    }
}