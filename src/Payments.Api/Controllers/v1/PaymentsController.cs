using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreatePayment;
using Payments.Application.Commands.v1.DeletePayment;
using Payments.Application.Commands.v1.UpdatePayment;
using Payments.Application.Queries.v1.GetPaymentById;
using Payments.Application.Queries.v1.GetPayments;

namespace Payments.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class PaymentsController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> GetPaymentsAsync([FromQuery] GetPaymentsQueryRequest request, [FromHeader(Name = "X-Profile-Id")] int profileId, CancellationToken cancellationToken)
    {
        var query = GetPaymentsQueryMapper.ToQuery(request, profileId);
        var payments = await _dispatcher.QueryAsync<GetPaymentsQuery, GetPaymentsQueryResponse>(query, cancellationToken);
        return Ok(payments);
    }

    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetPaymentByIdAsync([FromRoute] int paymentId, [FromHeader(Name = "X-Profile-Id")] int profileId, CancellationToken cancellationToken)
    {
        var payment = await _dispatcher.QueryAsync<GetPaymentByIdQuery, GetPaymentByIdQueryResponse>(new GetPaymentByIdQuery(paymentId, profileId), cancellationToken);
        if (payment is null)
        {
            return NotFound();
        }
        return Ok(payment);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaymentAsync([FromHeader(Name = "X-Profile-Id")] int profileId, [FromBody] CreatePaymentCommandRequest request, CancellationToken cancellationToken)
    {
        var command = CreatePaymentCommandMapper.ToCommand(request, profileId);
        var result = await _dispatcher.SendAsync<CreatePaymentCommand, CreatePaymentCommandResponse>(command, cancellationToken);
        return Created(nameof(GetPaymentByIdAsync), result);
    }

    [HttpPut("{paymentId}")]
    public async Task<IActionResult> UpdatePaymentAsync([FromRoute] int paymentId, [FromHeader(Name = "X-Profile-Id")] int profileId, [FromBody] UpdatePaymentCommandRequest request, CancellationToken cancellationToken)
    {
        var command = UpdatePaymentCommandMapper.ToCommand(request, paymentId, profileId);
        var result = await _dispatcher.SendAsync<UpdatePaymentCommand, UpdatePaymentCommandResponse>(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{paymentId}")]
    public async Task<IActionResult> DeletePaymentAsync([FromRoute] int paymentId, [FromHeader(Name = "X-Profile-Id")] int profileId, CancellationToken cancellationToken)
    {
        var command = new DeletePaymentCommand(paymentId, profileId);
        var result = await _dispatcher.SendAsync<DeletePaymentCommand, DeletePaymentCommandResponse>(command, cancellationToken);
        return Ok(result);
    }
}
