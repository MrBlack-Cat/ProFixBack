using Application.CQRS.Messages.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Messages.Queries.Requests;

public record GetAllMessagesByUserIdQuery(int UserId) : IRequest<ResponseModel<List<MessageListDto>>>;
