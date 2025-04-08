using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.FileUploads.DTOs;

public class UploadFileDto
{
    [Required]
    [FromForm(Name = "file")]
    public IFormFile File { get; set; }

    //[Required]
    //[FromForm(Name = "userId")]
    //public int UserId { get; set; }
}
