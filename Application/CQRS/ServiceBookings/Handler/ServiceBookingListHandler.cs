using Application.Common.Interfaces;
using Application.CQRS.ServiceBookings.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.ServiceBookings.Handler;

public class ServiceBookingListHandler
{

    public record struct ServiceBookingListQuery() : IRequest<ResponseModel<List<ServiceBookingListDto>>>;

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger)
    : IRequestHandler<ServiceBookingListQuery, ResponseModel<List<ServiceBookingListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<List<ServiceBookingListDto>>> Handle(ServiceBookingListQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _unitOfWork.ServiceBookingRepository.GetAllAsync();

            var responseDto = _mapper.Map<List<ServiceBookingListDto>>(bookings);


            #region ActivityLog
            await _activityLogger.LogAsync(
                userId: 0, 
                action: "List",
                entityType: "ServiceBooking", 
                entityId: 0, 
                performedBy: null, 
                description: "Listed all service bookings." 
            );
            #endregion


            return new ResponseModel<List<ServiceBookingListDto>>
            {
                Data = responseDto,
                IsSuccess = true
            };
        }
    }


}
