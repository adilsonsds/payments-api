using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreateProfile;
using Payments.Application.Commands.v1.DeleteProfile;
using Payments.Application.Queries.v1.GetProfileById;
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

    [HttpGet("{profileId}")]
    public async Task<IActionResult> GetProfileByIdAsync([FromRoute] int profileId, CancellationToken cancellationToken)
    {
        var profile = await _dispatcher.QueryAsync<GetProfileByIdQuery, GetProfileByIdQueryResponse>(new GetProfileByIdQuery(profileId), cancellationToken);
        if (profile is null)
        {
            return NotFound();
        }
        return Ok(profile);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProfileAsync([FromBody] CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.SendAsync<CreateProfileCommand, CreateProfileCommandResponse>(command, cancellationToken);
        return CreatedAtAction(nameof(GetProfileByIdAsync), new { profileId = result.Id }, result);
    }

    [HttpDelete("{profileId}")]
    public async Task<IActionResult> DeleteProfileAsync([FromRoute] int profileId, CancellationToken cancellationToken)
    {
        var command = new DeleteProfileCommand(profileId);
        var result = await _dispatcher.SendAsync<DeleteProfileCommand, DeleteProfileCommandResponse>(command, cancellationToken);
        return Ok(result);
    }
}

