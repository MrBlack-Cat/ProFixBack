using Application.CQRS.SupportTickets.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SupportTickets.Commands.Requests;

public class CreateSupportTicketRequest : IRequest<ResponseModel<CreateSupportTicketDto>>
{
    public CreateSupportTicketDto SupportTicketDto { get; set; }

    public CreateSupportTicketRequest(CreateSupportTicketDto supportTicketDto)
    {
        SupportTicketDto = supportTicketDto;
    }
}
