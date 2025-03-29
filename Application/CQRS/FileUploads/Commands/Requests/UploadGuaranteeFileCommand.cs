using Application.CQRS.FileUploads.DTOs;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CQRS.FileUploads.Commands.Requests;

public record UploadGuaranteeFileCommand(IFormFile File) : IRequest<ResponseModel<UploadFileResultDto>>;
