namespace Payments.Application.Queries.v1.GetBackup;

public record GetBackupQueryResponse
{
    public string FileName { get; init; } = string.Empty;
    public byte[] FileContent { get; init; } = Array.Empty<byte>();
    public string ContentType { get; init; } = "text/csv";
}
