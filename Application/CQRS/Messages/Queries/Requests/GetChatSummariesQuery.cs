using MediatR;
using Application.CQRS.Messages.DTOs;
using Common.GlobalResponse;

public record GetChatSummariesQuery(int UserId) : IRequest<ResponseModel<List<ChatSummaryDto>>>;