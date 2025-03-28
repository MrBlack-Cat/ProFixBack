using Application.CQRS.Messages.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Messages.Queries.Requests;

public record GetAllMessagesBetweenUsersQuery(int UserId1, int UserId2)
    : IRequest<ResponseModel<List<MessageListDto>>>;
