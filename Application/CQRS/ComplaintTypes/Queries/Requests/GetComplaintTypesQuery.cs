using Common.GlobalResponse;
using MediatR;
using Application.CQRS.ComplaintTypes.DTOs;

namespace Application.CQRS.ComplaintTypes.Queries.Requests;

public class GetComplaintTypesQuery : IRequest<ResponseModel<List<ComplaintTypeDto>>> { }
