using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Queries.v1.GetBackup;

namespace Payments.Api.Controllers.v1;

/// <summary>
/// Manages data backup operations
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class BackupController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;
    
    /// <summary>
    /// Generates and returns a data backup file
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Backup file</returns>
    /// <response code="200">Returns the backup file</response>
    [HttpGet]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBackupAsync(CancellationToken cancellationToken)
    {
        var query = new GetBackupQuery();
        var response = await _dispatcher.QueryAsync<GetBackupQuery, GetBackupQueryResponse>(query, cancellationToken);        
        return File(response.FileContent, response.ContentType, response.FileName);
    }
}