using Application.CQRS.SupportTickets.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SupportTickets.Commands.Requests;

public class DeleteSupportTicketRequest : IRequest<ResponseModel<DeleteSupportTicketDto>>
{
    public DeleteSupportTicketDto SupportTicketDto { get; set; }

    public DeleteSupportTicketRequest(DeleteSupportTicketDto supportTicketDto)
    {
        SupportTicketDto = supportTicketDto;
    }
}