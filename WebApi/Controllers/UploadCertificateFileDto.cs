using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.FileUploads.DTOs;

public class UploadCertificateFileDto
{
    [Required]
    [FromForm(Name = "file")]
    public IFormFile File { get; set; } = null!;
}
