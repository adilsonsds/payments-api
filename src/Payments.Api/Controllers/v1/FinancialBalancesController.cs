using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreateFinancialBalances;
using Payments.Application.Queries.v1.GetCategories;
using Payments.Application.Queries.v1.GetFinancialBalances;

namespace Payments.Api.Controllers.v1;

/// <summary>
/// Manages financial balances and categories operations
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class FinancialBalancesController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;

    /// <summary>
    /// Returns a list of financial balances
    /// </summary>
    /// <param name="query">Filter parameters for the financial balances query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of financial balances</returns>
    /// <response code="200">Returns the list of financial balances</response>
    [HttpGet]
    [ProducesResponseType(typeof(GetFinancialBalancesQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFinancialBalancesAsync([FromQuery] GetFinancialBalancesQuery query, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.QueryAsync<GetFinancialBalancesQuery, GetFinancialBalancesQueryResponse>(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates new financial balances
    /// </summary>
    /// <param name="command">Financial balances data to be created</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result</returns>
    /// <response code="200">Financial balances successfully created</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateFinancialBalancesCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateFinancialBalancesAsync([FromBody] CreateFinancialBalancesCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.SendAsync<CreateFinancialBalancesCommand, CreateFinancialBalancesCommandResponse>(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns a list of available categories
    /// </summary>
    /// <param name="query">Filter parameters for the categories query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of categories</returns>
    /// <response code="200">Returns the list of categories</response>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(GetCategoriesQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoriesAsync([FromQuery] GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.QueryAsync<GetCategoriesQuery, GetCategoriesQueryResponse>(query, cancellationToken);
        return Ok(result);
    }
}
