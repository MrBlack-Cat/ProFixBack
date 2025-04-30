using Application.Common.Interfaces;
using Application.CQRS.SupportTickets.DTOs;
using Application.CQRS.SupportTickets.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SupportTickets.Queries.Handlers;

public class GetSupportTicketByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger, IUserContext userContext) : IRequestHandler<GetSupportTicketByIdRequest, ResponseModel<GetSupportTicketByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityLoggerService _activityLogger = activityLogger;
    private readonly IUserContext _userContext = userContext;

    public async Task<ResponseModel<GetSupportTicketByIdDto>> Handle(GetSupportTicketByIdRequest request, CancellationToken cancellationToken)
    {
        var supportTicketEntity = await _unitOfWork.SupportTicketRepository.GetByIdAsync(request.Id);
        if (supportTicketEntity == null)
            throw new NotFoundException("Support ticket not found.");

        #region ActivityLog

        var currentUserId = _userContext.GetCurrentUserId();  

        await _activityLogger.LogAsync(
            userId: currentUserId.Value,  
            action: "View",  
            entityType: "SupportTicket",  
            entityId: supportTicketEntity.Id,  
            performedBy: currentUserId,  
            description: $"User {currentUserId.Value} viewed the support ticket with ID: {supportTicketEntity.Id}. Subject: {supportTicketEntity.Subject}"  
        );



        #endregion



        var responseDto = _mapper.Map<GetSupportTicketByIdDto>(supportTicketEntity);

        return new ResponseModel<GetSupportTicketByIdDto>
        {
            Data = responseDto,
            IsSuccess = true
        };
    }
}
