using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreatePlannedBalances;
using Payments.Application.Queries.v1.GetCategories;
using Payments.Application.Queries.v1.GetPlannedBalances;

namespace Payments.Api.Controllers.v1;

/// <summary>
/// Manages planned balances and categories operations
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class PlannedBalancesController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;

    /// <summary>
    /// Returns a list of planned balances
    /// </summary>
    /// <param name="query">Filter parameters for the planned balances query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of planned balances</returns>
    /// <response code="200">Returns the list of planned balances</response>
    [HttpGet]
    [ProducesResponseType(typeof(GetPlannedBalancesQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPlannedBalancesAsync([FromQuery] GetPlannedBalancesQuery query, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.QueryAsync<GetPlannedBalancesQuery, GetPlannedBalancesQueryResponse>(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates new planned balances
    /// </summary>
    /// <param name="command">Planned balances data to be created</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result</returns>
    /// <response code="200">Planned balances successfully created</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreatePlannedBalancesCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePlannedBalancesAsync([FromBody] CreatePlannedBalancesCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.SendAsync<CreatePlannedBalancesCommand, CreatePlannedBalancesCommandResponse>(command, cancellationToken);
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
