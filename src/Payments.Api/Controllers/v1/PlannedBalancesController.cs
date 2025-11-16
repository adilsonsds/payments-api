using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreatePlannedBalances;
using Payments.Application.Queries.v1.GetCategories;
using Payments.Application.Queries.v1.GetPlannedBalances;

namespace Payments.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class PlannedBalancesController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> GetPlannedBalancesAsync([FromQuery] GetPlannedBalancesQuery query, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.QueryAsync<GetPlannedBalancesQuery, GetPlannedBalancesQueryResponse>(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlannedBalancesAsync([FromBody] CreatePlannedBalancesCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.SendAsync<CreatePlannedBalancesCommand, CreatePlannedBalancesCommandResponse>(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategoriesAsync([FromQuery] GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.QueryAsync<GetCategoriesQuery, GetCategoriesQueryResponse>(query, cancellationToken);
        return Ok(result);
    }
}
