using Application.Common.Interfaces;
using Application.CQRS.ServiceBookings.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.ServiceBookings.Handler;

public class CreateServiceBookingHandler
{

    public record struct CreateServiceBookingCommand(CreateServiceBookingDto Dto) : IRequest<ResponseModel<CreateServiceBookingDto>>;


    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger)
    : IRequestHandler<CreateServiceBookingCommand, ResponseModel<CreateServiceBookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<CreateServiceBookingDto>> Handle(CreateServiceBookingCommand request, CancellationToken cancellationToken)
        {
            var newBooking = _mapper.Map<ServiceBooking>(request.Dto);

            await _unitOfWork.ServiceBookingRepository.AddAsync(newBooking);
            await _unitOfWork.SaveChangesAsync();

            #region ActivityLog
            await activityLogger.LogAsync
            (
                userId: newBooking.ClientProfileId,
                action: "Create",
                entityType: "ServiceBooking",
                entityId: newBooking.Id,
                performedBy: newBooking.ClientProfileId,
                description: $"Service booking created for service provider {newBooking.ServiceProviderProfileId} with description '{newBooking.Description}'. Scheduled Date: {newBooking.ScheduledDate?.ToString("yyyy-MM-dd HH:mm")}."

             );





            #endregion



            var responseDto = _mapper.Map<CreateServiceBookingDto>(newBooking);

            return new ResponseModel<CreateServiceBookingDto>
            {
                Data = responseDto,
                IsSuccess = true,
                Errors = []
            };
        }

    }
}
