using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreateBalances;
using Payments.Application.Queries.v1.GetCategories;
using Payments.Application.Queries.v1.GetBalances;

namespace Payments.Api.Controllers.v1;

/// <summary>
/// Manages balances and categories operations
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class BalancesController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;

    /// <summary>
    /// Returns a list of balances
    /// </summary>
    /// <param name="query">Filter parameters for the balances query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of balances</returns>
    /// <response code="200">Returns the list of balances</response>
    [HttpGet]
    [ProducesResponseType(typeof(GetBalancesQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBalancesAsync([FromQuery] GetBalancesQuery query, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.QueryAsync<GetBalancesQuery, GetBalancesQueryResponse>(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates new balances
    /// </summary>
    /// <param name="command">Balances data to be created</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result</returns>
    /// <response code="200">Balances successfully created</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateBalancesCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateBalancesAsync([FromBody] CreateBalancesCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.SendAsync<CreateBalancesCommand, CreateBalancesCommandResponse>(command, cancellationToken);
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
