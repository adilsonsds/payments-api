namespace Payments.Application.Commands.v1.CreateBackup;

public record CreateBackupCommandResponse
{
    public string FileName { get; set; } = string.Empty;
}

