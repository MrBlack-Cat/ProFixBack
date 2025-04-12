using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.FileUploads.DTOs;

public class UploadGuaranteeFileDto
{
    [Required]
    public IFormFile File { get; set; } = null!;
}
