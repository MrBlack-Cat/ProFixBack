namespace Application.CQRS.FileUploads.DTOs;

public class UploadFileResultDto
{
    public string FileName { get; set; } = null!;
    public string Url { get; set; } = null!;
}
