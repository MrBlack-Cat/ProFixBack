using Application.CQRS.ActivityLogs.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ActivityLogs.Commands.Requests;

public record CreateActivityLogCommand(CreateActivityLogDto Log)
    : IRequest<ResponseModel<CreateActivityLogDto>>;
