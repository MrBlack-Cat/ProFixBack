using Common.GlobalResponse;
using MediatR;
using Application.CQRS.Complaints.DTOs;

namespace Application.CQRS.Complaints.Queries.Requests;

public class GetComplaintListQuery : IRequest<ResponseModel<List<ComplaintListDto>>> { }
