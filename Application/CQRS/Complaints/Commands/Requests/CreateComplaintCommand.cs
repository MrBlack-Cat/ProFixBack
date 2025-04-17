using Application.CQRS.Complaints.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Complaints.Commands.Requests;

public record CreateComplaintCommand(int FromUserId, CreateComplaintDto Dto) : IRequest<ResponseModel<int>>;
