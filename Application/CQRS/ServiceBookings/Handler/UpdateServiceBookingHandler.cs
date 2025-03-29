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

public class UpdateServiceBookingHandler
{


    public record struct UpdateServiceBookingCommand(UpdateServiceBookingDto ServiceBookingDto) : IRequest<ResponseModel<UpdateServiceBookingDto>>;


    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger)
    : IRequestHandler<UpdateServiceBookingCommand, ResponseModel<UpdateServiceBookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<UpdateServiceBookingDto>> Handle(UpdateServiceBookingCommand request, CancellationToken cancellationToken)
        {
            var existingBooking = await _unitOfWork.ServiceBookingRepository.GetByIdAsync(request.ServiceBookingDto.Id);
            if (existingBooking == null)
                throw new NotFoundException("Service booking not found.");

            // dto-dan entitye 
            _mapper.Map(request.ServiceBookingDto, existingBooking);

            
            await _unitOfWork.ServiceBookingRepository.UpdateAsync(existingBooking);
            await _unitOfWork.SaveChangesAsync();


            #region ActivityLog
            await _activityLogger.LogAsync(
                userId: existingBooking.ClientProfileId,  
                action: "Update",                        
                entityType: "ServiceBooking",            
                entityId: existingBooking.Id,           
                performedBy: existingBooking.ClientProfileId, 
                description: $"Service booking updated for ClientProfileId {existingBooking.ClientProfileId} and ServiceProviderProfileId {existingBooking.ServiceProviderProfileId}. " +
                             $"Scheduled Date: {existingBooking.ScheduledDate}, Description: {existingBooking.Description}."
            );
            #endregion



            var updatedDto = _mapper.Map<UpdateServiceBookingDto>(existingBooking);

            return new ResponseModel<UpdateServiceBookingDto>
            {
                Data = updatedDto,
                IsSuccess = true
            };
        }
    }


}
