namespace Payments.Application.Queries.v1.GetPaymentById;

public record GetPaymentByIdQuery(int PaymentId, int ProfileId): IQuery<GetPaymentByIdQueryResponse>;
