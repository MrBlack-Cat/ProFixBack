using Application.Common.Interfaces;
using Application.CQRS.SupportTickets.Commands.Requests;
using Application.CQRS.SupportTickets.DTOs;
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

namespace Application.CQRS.SupportTickets.Commands.Handlers;

public class UpdateSupportTicketHandler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger, IUserContext userContext) : IRequestHandler<UpdateSupportTicketRequest, ResponseModel<UpdateSupportTicketDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityLoggerService _activityLogger = activityLogger;
    private readonly IUserContext _userContext = userContext;

    public async Task<ResponseModel<UpdateSupportTicketDto>> Handle(UpdateSupportTicketRequest request, CancellationToken cancellationToken)
    {
        var supportTicketEntity = await _unitOfWork.SupportTicketRepository.GetByIdAsync(request.SupportTicketDto.Id);
        if (supportTicketEntity == null)
            throw new NotFoundException("Support ticket not found.");

        // Entity üzərində dəyişiklikləri tətbiq edirik
        supportTicketEntity.Subject = request.SupportTicketDto.Subject;
        supportTicketEntity.Message = request.SupportTicketDto.Message;
        supportTicketEntity.StatusId = request.SupportTicketDto.StatusId;
        supportTicketEntity.UpdatedBy = request.SupportTicketDto.UpdatedBy;

        // Dəyişiklikləri bazaya qeyd edirik
        await _unitOfWork.SupportTicketRepository.UpdateAsync(supportTicketEntity);
        await _unitOfWork.SaveChangesAsync();


        #region ActivityLog

        var currentUserId = _userContext.GetCurrentUserId();  

        await _activityLogger.LogAsync(
            userId: currentUserId.Value,  
            action: "Update",  
            entityType: "SupportTicket",  
            entityId: supportTicketEntity.Id,  
            performedBy: currentUserId,  
            description: $"User {currentUserId.Value} updated the support ticket with ID: {supportTicketEntity.Id}. Subject: {supportTicketEntity.Subject}, StatusId: {supportTicketEntity.StatusId}"
        );

        #endregion


        var responseDto = _mapper.Map<UpdateSupportTicketDto>(supportTicketEntity);

        return new ResponseModel<UpdateSupportTicketDto>
        {
            Data = responseDto,
            IsSuccess = true
        };
    }
}