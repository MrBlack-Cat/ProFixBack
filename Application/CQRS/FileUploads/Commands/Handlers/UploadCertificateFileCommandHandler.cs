using Application.CQRS.FileUploads.Commands.Requests;
using Application.CQRS.FileUploads.DTOs;
using Application.Common.Interfaces;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Common.Interfaces;

namespace Application.CQRS.FileUploads.Commands.Handlers;

public class UploadCertificateFileCommandHandler : IRequestHandler<UploadCertificateFileCommand, ResponseModel<UploadFileResultDto>>
{
    private readonly ICloudStorageService _cloudStorageService;
    private readonly IUserContext _userContext;

    public UploadCertificateFileCommandHandler(
        ICloudStorageService cloudStorageService,
        IUserContext userContext)
    {
        _cloudStorageService = cloudStorageService;
        _userContext = userContext;
    }

    public async Task<ResponseModel<UploadFileResultDto>> Handle(UploadCertificateFileCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
            throw new BadRequestException("No file selected.");

        if (request.File.ContentType != "application/pdf")
            throw new BadRequestException("Only PDF files are allowed.");

        var userId = _userContext.MustGetUserId();
        var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";

        using var stream = request.File.OpenReadStream();
        var url = await _cloudStorageService.UploadFileAsync(stream, fileName, request.File.ContentType);

        return new ResponseModel<UploadFileResultDto>
        {
            Data = new UploadFileResultDto
            {
                FileName = fileName,
                Url = url
            },
            IsSuccess = true
        };
    }
}
