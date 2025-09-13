namespace Payments.Application.Queries.v1.GetPayments;

public static class GetPaymentsQueryMapper
{
    public static GetPaymentsQuery ToQuery(this GetPaymentsQueryRequest request, int profileId) =>
        new(
            ProfileId: profileId,
            FromDate: request.FromDate,
            ToDate: request.ToDate,
            PageNumber: request.PageNumber,
            PageSize: request.PageSize
        );
}