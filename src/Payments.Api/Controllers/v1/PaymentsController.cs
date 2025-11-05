using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreatePayment;
using Payments.Application.Commands.v1.DeletePayment;
using Payments.Application.Commands.v1.UpdatePayment;
using Payments.Application.Queries.v1.GetPaymentById;
using Payments.Application.Queries.v1.GetPayments;
using Payments.Application.Queries.v1.GetPaymentsSummary;

namespace Payments.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class PaymentsController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> GetPaymentsAsync([FromQuery] GetPaymentsQuery query, CancellationToken cancellationToken = default)
    {
        var payments = await _dispatcher.QueryAsync<GetPaymentsQuery, GetPaymentsQueryResponse>(query, cancellationToken);
        return Ok(payments);
    }

    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetPaymentByIdAsync([FromRoute] int paymentId, CancellationToken cancellationToken)
    {
        var payment = await _dispatcher.QueryAsync<GetPaymentByIdQuery, GetPaymentByIdQueryResponse>(new GetPaymentByIdQuery(paymentId), cancellationToken);
        if (payment is null)
        {
            return NotFound();
        }
        return Ok(payment);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaymentAsync([FromBody] CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.SendAsync<CreatePaymentCommand, CreatePaymentCommandResponse>(command, cancellationToken);
        return Created(nameof(GetPaymentByIdAsync), result);
    }

    [HttpPut("{paymentId}")]
    public async Task<IActionResult> UpdatePaymentAsync([FromRoute] int paymentId, [FromBody] UpdatePaymentCommandRequest commandRequest, CancellationToken cancellationToken)
    {
        var command = UpdatePaymentCommandMapper.ToCommand(commandRequest, paymentId);
        var result = await _dispatcher.SendAsync<UpdatePaymentCommand, UpdatePaymentCommandResponse>(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{paymentId}")]
    public async Task<IActionResult> DeletePaymentAsync([FromRoute] int paymentId, CancellationToken cancellationToken)
    {
        var command = new DeletePaymentCommand(paymentId);
        var result = await _dispatcher.SendAsync<DeletePaymentCommand, DeletePaymentCommandResponse>(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetPaymentsSummaryAsync([FromQuery] GetPaymentsSummaryQuery query, CancellationToken cancellationToken)
    {
        var summary = await _dispatcher.QueryAsync<GetPaymentsSummaryQuery, GetPaymentsSummaryQueryResponse>(query, cancellationToken);
        return Ok(summary);
    }
}
