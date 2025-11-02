using System.Globalization;
using System.Text;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Payments.Infra;
using Shared.Abstractions;

namespace Payments.Application.Commands.v1.CreateBackup;

public class CreateBackupCommandHandler(
    PaymentsDbContext context,
    IFileStorageBackupService fileStorageBackupService) 
    : ICommandHandler<CreateBackupCommand, CreateBackupCommandResponse>
{
    private readonly PaymentsDbContext _context = context;
    private readonly IFileStorageBackupService _fileStorageBackupService = fileStorageBackupService;

    public async Task<CreateBackupCommandResponse> HandleAsync(CreateBackupCommand command, CancellationToken cancellationToken)
    {
        var payments = await _context.Payments
            .Include(p => p.Profile)
            .ToListAsync(cancellationToken: cancellationToken);

        var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        await csv.WriteRecordsAsync(payments.Select(p => new
        {
            Id = p.Id,
            Content = p.Content,
            Description = p.Description,
            Amount = p.Amount,
            PaymentDate = p.PaymentDate,
            Completed = p.Completed,
            ProfileId = p.Profile.Id,
            ProfileName = p.Profile.Name,
            CreatedAt = p.CreatedAt
        }), cancellationToken);

        await writer.FlushAsync(cancellationToken);
        memoryStream.Position = 0;

        var fileName = $"payments_backup_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";

        await _fileStorageBackupService.SaveBackupAsync(
            memoryStream,
            "text/csv",
            "/backups/",
            fileName,
            command.ApiKey,
            cancellationToken);

        return new CreateBackupCommandResponse
        {
            FileName = fileName
        };
    }
}