using Application.Common.Interfaces;
using Application.CQRS.ServiceBookings.DTOs;
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

namespace Application.CQRS.ServiceBookings.Handler;

public class GetServiceBookingByIdHandler
{

    public record struct GetServiceBookingByIdQuery(int Id) : IRequest<ResponseModel<GetServiceBookingByIdDto>>;


    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger)
    : IRequestHandler<GetServiceBookingByIdQuery, ResponseModel<GetServiceBookingByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;
        public async Task<ResponseModel<GetServiceBookingByIdDto>> Handle(GetServiceBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.ServiceBookingRepository.GetByIdAsync(request.Id);
            if (booking == null)
                throw new NotFoundException("Service booking not found.");

            var responseDto = _mapper.Map<GetServiceBookingByIdDto>(booking);

            #region ActivityLog
            await _activityLogger.LogAsync(
                userId: booking.ClientProfileId,  
                action: "Get",                    
                entityType: "ServiceBooking",
                entityId: booking.Id,        
                performedBy: booking.ClientProfileId,  
                description: $"Service booking details retrieved for ClientProfileId {booking.ClientProfileId} and ServiceProviderProfileId {booking.ServiceProviderProfileId}. " +
                             $"Scheduled Date: {booking.ScheduledDate}, Description: {booking.Description}."
            );
            #endregion


            return new ResponseModel<GetServiceBookingByIdDto>
            {
                Data = responseDto,
                IsSuccess = true
            };
        }
    }


}
