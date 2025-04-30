using Application.Common.Interfaces;
using Application.CQRS.FileUploads.Commands.Requests;
using Application.CQRS.FileUploads.DTOs;
using Common.Exceptions;
using Common.GlobalResponse;
using Common.Interfaces;
using MediatR;

namespace Application.CQRS.FileUploads.Commands.Handlers;

public class UploadPostImageCommandHandler : IRequestHandler<UploadPostImageCommand, ResponseModel<UploadFileResultDto>>
{
    private readonly ICloudStorageService _cloudStorageService;
    private readonly IUserContext _userContext;

    public UploadPostImageCommandHandler(
        ICloudStorageService cloudStorageService,
        IUserContext userContext)
    {
        _cloudStorageService = cloudStorageService;
        _userContext = userContext;
    }

    public async Task<ResponseModel<UploadFileResultDto>> Handle(UploadPostImageCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
            throw new BadRequestException("No file selected.");

        if (!request.File.ContentType.StartsWith("image/"))
            throw new BadRequestException("Only image files are allowed.");

        var userId = _userContext.MustGetUserId();
        var fileName = $"post_{userId}_{Guid.NewGuid()}_{request.File.FileName}";

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
