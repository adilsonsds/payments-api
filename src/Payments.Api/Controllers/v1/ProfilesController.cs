using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreateProfile;
using Payments.Application.Commands.v1.DeleteProfile;
using Payments.Application.Queries.v1.GetProfileById;
using Payments.Application.Queries.v1.GetProfiles;

namespace Payments.Api.Controllers.v1;

/// <summary>
/// Manages user profile-related operations
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class ProfilesController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;

    /// <summary>
    /// Returns a list of profiles
    /// </summary>
    /// <param name="query">Filter parameters for the profiles query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of profiles</returns>
    /// <response code="200">Returns the list of profiles</response>
    [HttpGet]
    [ProducesResponseType(typeof(GetProfilesQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfilesAsync([FromQuery] GetProfilesQuery query, CancellationToken cancellationToken)
    {
        var profiles = await _dispatcher.QueryAsync<GetProfilesQuery, GetProfilesQueryResponse>(query, cancellationToken);
        return Ok(profiles);
    }

    /// <summary>
    /// Returns the details of a specific profile
    /// </summary>
    /// <param name="profileId">Profile ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Profile details</returns>
    /// <response code="200">Returns the found profile</response>
    /// <response code="404">Profile not found</response>
    [HttpGet("{profileId}")]
    [ProducesResponseType(typeof(GetProfileByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileByIdAsync([FromRoute] int profileId, CancellationToken cancellationToken)
    {
        var profile = await _dispatcher.QueryAsync<GetProfileByIdQuery, GetProfileByIdQueryResponse>(new GetProfileByIdQuery(profileId), cancellationToken);
        if (profile is null)
        {
            return NotFound();
        }
        return Ok(profile);
    }

    /// <summary>
    /// Creates a new profile
    /// </summary>
    /// <param name="command">Profile data to be created</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created profile</returns>
    /// <response code="201">Profile successfully created</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateProfileCommandResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProfileAsync([FromBody] CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.SendAsync<CreateProfileCommand, CreateProfileCommandResponse>(command, cancellationToken);
        return CreatedAtAction(nameof(GetProfileByIdAsync), new { profileId = result.Id }, result);
    }

    /// <summary>
    /// Deletes a profile
    /// </summary>
    /// <param name="profileId">Profile ID to be deleted</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result</returns>
    /// <response code="200">Profile successfully deleted</response>
    [HttpDelete("{profileId}")]
    [ProducesResponseType(typeof(DeleteProfileCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProfileAsync([FromRoute] int profileId, CancellationToken cancellationToken)
    {
        var command = new DeleteProfileCommand(profileId);
        var result = await _dispatcher.SendAsync<DeleteProfileCommand, DeleteProfileCommandResponse>(command, cancellationToken);
        return Ok(result);
    }
}

