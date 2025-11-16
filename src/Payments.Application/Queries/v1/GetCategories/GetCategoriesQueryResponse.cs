namespace Payments.Application.Queries.v1.GetCategories;

public record GetCategoriesQueryResponse(
    List<string> Categories
);