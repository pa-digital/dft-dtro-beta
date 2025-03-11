namespace DfT.DTRO.Services;

public interface IGoogleCloudStorageService
{
    Task UploadStreamAsync(Stream dataStream, string destinationObjectName);
}