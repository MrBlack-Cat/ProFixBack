using Application.Common.Interfaces;
using Application.CQRS.SupportTickets.DTOs;
using Application.CQRS.SupportTickets.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using Infrastructure.Services;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SupportTickets.Queries.Handlers;

public class SupportTicketListHandler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger, IUserContext userContext) : IRequestHandler<SupportTicketListRequest, ResponseModel<List<SupportTicketListDto>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityLoggerService _activityLogger = activityLogger;
    private readonly IUserContext _userContext = userContext;

    public async Task<ResponseModel<List<SupportTicketListDto>>> Handle(SupportTicketListRequest request, CancellationToken cancellationToken)
    {
        var supportTickets = await _unitOfWork.SupportTicketRepository.GetAllAsync();


        #region ActivityLog

        var currentUserId = _userContext.GetCurrentUserId();
        var allTicketIds = supportTickets.Select(x => x.Id).ToList();
        await _activityLogger.LogAsync(
            userId: currentUserId.Value,  
            action: "View",  
            entityType: "SupportTicket",  
            entityId: 0,
            entityIds: allTicketIds,
            performedBy: currentUserId,  
            description: $"User {currentUserId.Value} viewed the list of support tickets."  
        );
        #endregion



        var responseDtos = _mapper.Map<List<SupportTicketListDto>>(supportTickets);

        return new ResponseModel<List<SupportTicketListDto>>
        {
            Data = responseDtos,
            IsSuccess = true
        };
    }
}