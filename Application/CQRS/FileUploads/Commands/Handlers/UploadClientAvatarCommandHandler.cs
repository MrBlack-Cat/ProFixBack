﻿using Application.Common.Interfaces;
using Application.CQRS.FileUploads.Commands.Requests;
using Application.CQRS.FileUploads.DTOs;
using Common.Exceptions;
using Common.GlobalResponse;
using Common.Interfaces;
using MediatR;

namespace Application.CQRS.FileUploads.Commands.Handlers;

public class UploadClientAvatarCommandHandler : IRequestHandler<UploadClientAvatarCommand, ResponseModel<UploadFileResultDto>>
{
    private readonly ICloudStorageService _cloudStorageService;
    private readonly IUserContext _userContext;

    public UploadClientAvatarCommandHandler(
        ICloudStorageService cloudStorageService,
        IUserContext userContext)
    {
        _cloudStorageService = cloudStorageService;
        _userContext = userContext;
    }

    public async Task<ResponseModel<UploadFileResultDto>> Handle(UploadClientAvatarCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
            throw new BadRequestException("Select File.");

        var userId = _userContext.MustGetUserId(); 
        var fileName = $"{userId}_avatar_{Guid.NewGuid()}_{request.File.FileName}";

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
