using Application.CQRS.ClientProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ClientProfiles.Queries.Requests;

public class GetAllClientProfilesQuery : IRequest<ResponseModel<List<ClientProfileDto>>>;
