using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreateProfile;
using Payments.Application.Commands.v1.DeleteProfile;
using Payments.Application.Queries.v1.GetProfiles;

namespace Payments.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class ProfilesController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> GetProfilesAsync([FromQuery] GetProfilesQuery query, CancellationToken cancellationToken)
    {
        var profiles = await _dispatcher.QueryAsync<GetProfilesQuery, GetProfilesQueryResponse>(query, cancellationToken);
        return Ok(profiles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProfileAsync([FromBody] CreateProfileCommand command, CancellationToken cancellationToken)
    {
        await _dispatcher.SendAsync(command, cancellationToken);
        return CreatedAtAction(nameof(GetProfilesAsync), null);
    }

    [HttpDelete("{profileId}")]
    public async Task<IActionResult> DeleteProfileAsync([FromRoute] int profileId, CancellationToken cancellationToken)
    {
        var command = new DeleteProfileCommand(profileId);
        await _dispatcher.SendAsync(command, cancellationToken);
        return NoContent();
    }
}

