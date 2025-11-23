using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Queries.v1.GetBackup;

namespace Payments.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class BackupController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;
    
    [HttpGet]
    public async Task<IActionResult> GetBackupAsync(CancellationToken cancellationToken)
    {
        var query = new GetBackupQuery();
        var response = await _dispatcher.QueryAsync<GetBackupQuery, GetBackupQueryResponse>(query, cancellationToken);        
        return File(response.FileContent, response.ContentType, response.FileName);
    }
}