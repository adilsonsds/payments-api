using Microsoft.AspNetCore.Mvc;
using Payments.Application;
using Payments.Application.Commands.v1.CreateBackup;

namespace Payments.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class BackupController(CqrsDispatcher dispatcher) : ControllerBase
{
    private readonly CqrsDispatcher _dispatcher = dispatcher;
    
    [HttpPost]
    public async Task<IActionResult> CreateBackup([FromBody] CreateBackupCommand command, CancellationToken cancellationToken)
    {
        var response = await _dispatcher.SendAsync<CreateBackupCommand, CreateBackupCommandResponse>(command, cancellationToken);
        return Ok(response);
    }
}