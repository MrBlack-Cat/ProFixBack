using Application.CQRS.Messages.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Messages.Queries.Requests;

public record GetMessageByIdQuery(int Id) : IRequest<ResponseModel<GetMessageByIdDto>>;
