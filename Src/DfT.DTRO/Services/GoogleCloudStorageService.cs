using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace DfT.DTRO.Services;

public class GoogleCloudStorageService : IGoogleCloudStorageService
{
    private readonly string _bucketName;
    private readonly StorageClient _storageClient;
    
    
    public GoogleCloudStorageService(IConfiguration configuration)
    {
        var storageConfig = configuration.GetSection("GoogleCloudStorage");
        _bucketName = storageConfig["BucketName"];

        var projectId = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT");
        bool isRunningInGoogleCloud = !string.IsNullOrEmpty(projectId);

        var serviceAccountPath = isRunningInGoogleCloud ? null : storageConfig["ServiceAccountPath"];

        _storageClient = !string.IsNullOrEmpty(serviceAccountPath)
            ? StorageClient.Create(GoogleCredential.FromFile(serviceAccountPath))
            : StorageClient.Create();
    }
    
    
    public async Task UploadStreamAsync(Stream dataStream, string destinationObjectName)
    {
        await _storageClient.UploadObjectAsync(_bucketName, destinationObjectName, null, dataStream);
        Console.WriteLine($"Uploaded stream to {_bucketName} as {destinationObjectName}");
    }
}