namespace Payments.Application.Commands.v1.CreateBackup;

public record CreateBackupCommand(string ApiKey) : ICommand<CreateBackupCommandResponse>;
