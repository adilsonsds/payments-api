namespace Shared.Abstractions;

public interface IFileStorageBackupService
{
    Task<string> SaveBackupAsync(Stream fileStream, string fileType, string filePath, string fileName, string apiKey, CancellationToken cancellationToken);
}