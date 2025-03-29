namespace Common.Interfaces;

public interface ICloudStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
    Task DeleteFileAsync(string fileName);
    string GetPublicUrl(string fileName);
}
