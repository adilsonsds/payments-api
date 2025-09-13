using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetProfiles;

public class GetProfilesQueryHandler(PaymentsDbContext context)
    : IQueryHandler<GetProfilesQuery, GetProfilesQueryResponse>
{
    private readonly PaymentsDbContext _context = context;

    public async Task<GetProfilesQueryResponse> HandleAsync(GetProfilesQuery query, CancellationToken cancellationToken)
    {
        var responseItems = await _context.Profiles
            .Select(p => new GetProfilesQueryResponseItem(
                p.Id,
                p.Name
            ))
            .ToListAsync(cancellationToken);

        return new GetProfilesQueryResponse(responseItems);
    }
}
