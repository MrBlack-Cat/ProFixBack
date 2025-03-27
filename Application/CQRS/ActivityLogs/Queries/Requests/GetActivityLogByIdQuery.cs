using Application.CQRS.ActivityLogs.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ActivityLogs.Queries.Requests;

public record GetActivityLogByIdQuery(int Id) : IRequest<ResponseModel<GetActivityLogByIdDto>>;
