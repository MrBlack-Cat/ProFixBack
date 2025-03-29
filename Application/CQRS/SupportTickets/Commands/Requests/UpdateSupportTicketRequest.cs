using Application.CQRS.SupportTickets.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SupportTickets.Commands.Requests;

public class UpdateSupportTicketRequest : IRequest<ResponseModel<UpdateSupportTicketDto>>
{
    public UpdateSupportTicketDto SupportTicketDto { get; set; }

    public UpdateSupportTicketRequest(UpdateSupportTicketDto supportTicketDto)
    {
        SupportTicketDto = supportTicketDto;
    }
}