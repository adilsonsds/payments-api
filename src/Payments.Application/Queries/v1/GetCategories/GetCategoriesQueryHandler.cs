using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetCategories;

public class GetCategoriesQueryHandler(PaymentsDbContext dbContext) : IQueryHandler<GetCategoriesQuery, GetCategoriesQueryResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<GetCategoriesQueryResponse> HandleAsync(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _dbContext.PlannedBalances
            .Where(pb => 
                pb.Profile.Id == query.ProfileId && 
                pb.Year == query.Year && 
                pb.Month == query.Month)
            .Select(pb => pb.Category)
            .Distinct()
            .ToListAsync(cancellationToken);

        return new GetCategoriesQueryResponse(categories);
    }
}