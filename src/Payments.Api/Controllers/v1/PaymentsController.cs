using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreatePayment;
using Payments.Application.Commands.v1.DeletePayment;
using Payments.Application.Commands.v1.UpdatePayment;
using Payments.Application.Queries.v1.GetPaymentById;
using Payments.Application.Queries.v1.GetPayments;
using Payments.Application.Queries.v1.GetPaymentsSummary;

namespace Payments.Api.Controllers.v1;

/// <summary>
/// Manages payment-related operations
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class PaymentsController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;

    /// <summary>
    /// Returns a list of payments
    /// </summary>
    /// <param name="query">Filter parameters for the payments query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of payments</returns>
    /// <response code="200">Returns the list of payments</response>
    [HttpGet]
    [ProducesResponseType(typeof(GetPaymentsQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentsAsync([FromQuery] GetPaymentsQuery query, CancellationToken cancellationToken = default)
    {
        var payments = await _dispatcher.QueryAsync<GetPaymentsQuery, GetPaymentsQueryResponse>(query, cancellationToken);
        return Ok(payments);
    }

    /// <summary>
    /// Returns a summary of payments grouped by category
    /// </summary>
    /// <param name="query">Filter parameters for the summary</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment summary</returns>
    /// <response code="200">Returns the payment summary</response>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(GetPaymentsSummaryQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentsSummaryAsync([FromQuery] GetPaymentsSummaryQuery query, CancellationToken cancellationToken)
    {
        var summary = await _dispatcher.QueryAsync<GetPaymentsSummaryQuery, GetPaymentsSummaryQueryResponse>(query, cancellationToken);
        return Ok(summary);
    }

    /// <summary>
    /// Returns the details of a specific payment
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment details</returns>
    /// <response code="200">Returns the found payment</response>
    /// <response code="404">Payment not found</response>
    [HttpGet("{paymentId}")]
    [ProducesResponseType(typeof(GetPaymentByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentByIdAsync([FromRoute] int paymentId, CancellationToken cancellationToken)
    {
        var payment = await _dispatcher.QueryAsync<GetPaymentByIdQuery, GetPaymentByIdQueryResponse>(new GetPaymentByIdQuery(paymentId), cancellationToken);
        if (payment is null)
        {
            return NotFound();
        }
        return Ok(payment);
    }

    /// <summary>
    /// Creates a new payment
    /// </summary>
    /// <param name="command">Payment data to be created</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created payment</returns>
    /// <response code="201">Payment successfully created</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreatePaymentCommandResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePaymentAsync([FromBody] CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.SendAsync<CreatePaymentCommand, CreatePaymentCommandResponse>(command, cancellationToken);
        return Created(nameof(GetPaymentByIdAsync), result);
    }

    /// <summary>
    /// Updates an existing payment
    /// </summary>
    /// <param name="paymentId">Payment ID to be updated</param>
    /// <param name="commandRequest">New payment data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated payment</returns>
    /// <response code="200">Payment successfully updated</response>
    [HttpPut("{paymentId}")]
    [ProducesResponseType(typeof(UpdatePaymentCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePaymentAsync([FromRoute] int paymentId, [FromBody] UpdatePaymentCommandRequest commandRequest, CancellationToken cancellationToken)
    {
        var command = UpdatePaymentCommandMapper.ToCommand(commandRequest, paymentId);
        var result = await _dispatcher.SendAsync<UpdatePaymentCommand, UpdatePaymentCommandResponse>(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a payment
    /// </summary>
    /// <param name="paymentId">Payment ID to be deleted</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result</returns>
    /// <response code="200">Payment successfully deleted</response>
    [HttpDelete("{paymentId}")]
    [ProducesResponseType(typeof(DeletePaymentCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeletePaymentAsync([FromRoute] int paymentId, CancellationToken cancellationToken)
    {
        var command = new DeletePaymentCommand(paymentId);
        var result = await _dispatcher.SendAsync<DeletePaymentCommand, DeletePaymentCommandResponse>(command, cancellationToken);
        return Ok(result);
    }
}
