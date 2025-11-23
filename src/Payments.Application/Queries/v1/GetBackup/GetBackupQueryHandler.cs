using System.Globalization;
using System.Text;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Payments.Infra;

namespace Payments.Application.Queries.v1.GetBackup;

public class GetBackupQueryHandler(PaymentsDbContext context) 
    : IQueryHandler<GetBackupQuery, GetBackupQueryResponse>
{
    private readonly PaymentsDbContext _context = context;
    
    public async Task<GetBackupQueryResponse> HandleAsync(GetBackupQuery query, CancellationToken cancellationToken)
    {
        var payments = await _context.Payments
            .Include(p => p.Profile)
            .ToListAsync(cancellationToken: cancellationToken);

        var memoryStream = new MemoryStream();
        await using var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

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
        
        var fileContent = memoryStream.ToArray();
        var fileName = $"payments_backup_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";

        return new GetBackupQueryResponse
        {
            FileName = fileName,
            FileContent = fileContent,
            ContentType = "text/csv"
        };
    }
}
