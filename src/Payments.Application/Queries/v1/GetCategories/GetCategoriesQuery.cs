namespace Payments.Application.Queries.v1.GetCategories;

public record GetCategoriesQuery : IQuery<GetCategoriesQueryResponse>
{
    public required int ProfileId { get; init; }
    public required int Year { get; init; }
    public required int Month { get; init; }
}
