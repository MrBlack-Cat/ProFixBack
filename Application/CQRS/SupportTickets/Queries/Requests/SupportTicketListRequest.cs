using Application.CQRS.SupportTickets.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SupportTickets.Queries.Requests;

public class SupportTicketListRequest : IRequest<ResponseModel<List<SupportTicketListDto>>>
{
    // filter ucun parametr lazim olsa elave edecem 
}
