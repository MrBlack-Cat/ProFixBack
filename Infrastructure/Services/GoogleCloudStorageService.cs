using Common.Interfaces;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class GoogleCloudStorageService : ICloudStorageService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public GoogleCloudStorageService(IConfiguration configuration)
    {
        _bucketName = configuration["GoogleCloud:BucketName"]!;
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", configuration["GoogleCloud:CredentialsPath"]);

        _storageClient = StorageClient.Create();
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var result = await _storageClient.UploadObjectAsync(
            bucket: _bucketName,
            objectName: fileName,
            contentType: contentType,
            source: fileStream
        );

        return $"https://storage.googleapis.com/{_bucketName}/{fileName}";
    }

    public async Task DeleteFileAsync(string fileName)
    {
        await _storageClient.DeleteObjectAsync(_bucketName, fileName);
    }

    public string GetPublicUrl(string fileName)
    {
        return $"https://storage.googleapis.com/{_bucketName}/{fileName}";
    }
}
