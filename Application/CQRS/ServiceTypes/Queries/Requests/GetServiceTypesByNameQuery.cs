using Application.CQRS.ServiceTypes.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.ServiceTypes.Queries.Requests
{
    public record GetServiceTypesByNameQuery(string Name) : IRequest<ResponseModel<List<ServiceTypeListDto>>>;

}
