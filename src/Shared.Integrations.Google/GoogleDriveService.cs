using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Shared.Abstractions;
using File = Google.Apis.Drive.v3.Data.File;

namespace Shared.Integrations.Google;

public class GoogleDriveService : IFileStorageBackupService
{
    public async Task<string> SaveBackupAsync(Stream fileStream, string fileType, string filePath, string fileName, string apiKey, CancellationToken cancellationToken)
    {
        try
        {
            var credential = GoogleCredential.FromAccessToken(apiKey)
                .CreateScoped(DriveService.Scope.DriveFile);

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Payments API Backup"
            });
            
            var fileMetadata = new File()
            {
                Name = fileName,
                Parents = []
            };

            fileStream.Position = 0;
            var request = service.Files.Create(fileMetadata, fileStream, fileType);
            request.Fields = "id";

            var result = await request.UploadAsync(cancellationToken);
            if (result.Status == UploadStatus.Failed)
            {
                throw new Exception($"Upload failed: {result.Exception?.Message}");
            }

            return request.ResponseBody.Id;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao fazer upload para Google Drive: {ex.Message}", ex);
        }
    }
}
