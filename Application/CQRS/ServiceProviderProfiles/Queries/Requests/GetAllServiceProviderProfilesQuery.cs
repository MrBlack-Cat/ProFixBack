using Application.CQRS.ServiceProviderProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.ServiceProviderProfiles.Queries.Requests;

public record GetAllServiceProviderProfilesQuery() : IRequest<ResponseModel<List<ServiceProviderProfileListDto>>>;
