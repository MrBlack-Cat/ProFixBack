using Application.CQRS.SupportTickets.DTOs;
using Common.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.CQRS.SupportTickets.Commands.Responses;

public class UpdateSupportTicketResponse : ResponseModel<UpdateSupportTicketDto>
{
    public UpdateSupportTicketResponse(UpdateSupportTicketDto data)
    {
        Data = data;
        IsSuccess = true;
    }
}