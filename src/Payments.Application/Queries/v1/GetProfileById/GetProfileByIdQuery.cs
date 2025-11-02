namespace Payments.Application.Queries.v1.GetProfileById;

public record GetProfileByIdQuery(int ProfileId) : IQuery<GetProfileByIdQueryResponse>;