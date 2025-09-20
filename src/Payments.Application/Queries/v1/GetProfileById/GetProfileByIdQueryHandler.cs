using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetProfileById;

public class GetProfileByIdQueryHandler(PaymentsDbContext dbContext) : IQueryHandler<GetProfileByIdQuery, GetProfileByIdQueryResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<GetProfileByIdQueryResponse> HandleAsync(GetProfileByIdQuery query, CancellationToken cancellationToken)
    {
        var profile = await _dbContext.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == query.ProfileId, cancellationToken)
            ?? throw new KeyNotFoundException($"Profile with ID {query.ProfileId} not found.");

        return new GetProfileByIdQueryResponse(profile.Id, profile.Name);
    }
}