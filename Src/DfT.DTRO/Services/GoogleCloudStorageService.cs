using Google.Cloud.Storage.V1;

public class GoogleCloudStorageService : IGoogleCloudStorageService
{
    private readonly string _bucketName = "dtro-error-reports-dev";
    private readonly StorageClient _storageClient;

    public GoogleCloudStorageService()
    {
        _storageClient = StorageClient.Create();
    }

    public async Task UploadFileAsync(string filepath)
    {
        using var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        var filename = $"{Guid.NewGuid()}_{Path.GetFileName(filepath)}";
        await _storageClient.UploadObjectAsync(_bucketName, filename, null, fileStream);
    }
}