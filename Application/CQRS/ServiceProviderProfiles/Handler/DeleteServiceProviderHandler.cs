using Application.Common.Interfaces;
using Application.CQRS.ServiceProviderProfiles.DTOs;
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

namespace Application.CQRS.ServiceProviderProfiles.Handler;

public class DeleteServiceProviderHandler
{



    public record struct DeleteServiceProviderProfileCommand(DeleteServiceProviderProfileDto Dto) : IRequest<ResponseModel<DeleteServiceProviderProfileDto>>;



    public sealed class DeleteServiceProviderProfileHandler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger)
    : IRequestHandler<DeleteServiceProviderProfileCommand, ResponseModel<DeleteServiceProviderProfileDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<DeleteServiceProviderProfileDto>> Handle(DeleteServiceProviderProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByIdAsync(request.Dto.Id);
            if (profile == null)
                throw new NotFoundException("Service Provider Profile not found.");

            // Soft delete tətbiq edirik
            profile.DeletedBy = request.Dto.DeletedByUserId;
            profile.DeletedAt = DateTime.UtcNow;
            profile.DeletedReason = request.Dto.Reason;

            await _unitOfWork.ServiceProviderProfileRepository.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();


            #region ActivityLog

            await _activityLogger.LogAsync(
                userId: request.Dto.DeletedByUserId.Value,  
                action: "Delete",
                entityType: "ServiceProviderProfile",
                entityId: profile.Id,  
                performedBy: request.Dto.DeletedByUserId,  
                description: $"Service provider profile with Id {profile.Id} deleted. Reason: {request.Dto.Reason}"
            );

            #endregion


            // AutoMapper ilə DTO-ya çeviririk
            var responseDto = _mapper.Map<DeleteServiceProviderProfileDto>(profile);

            return new ResponseModel<DeleteServiceProviderProfileDto>
            {
                Data = responseDto,
                IsSuccess = true,
                Errors = [],
            };
        }




    }
}
