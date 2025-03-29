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

public class UpdateServiceProviderHandlerProfile
{
    public record struct UpdateServiceProviderProfileCommand(UpdateServiceProviderProfileDto Dto) : IRequest<ResponseModel<UpdateServiceProviderProfileDto>>;



    public sealed class UpdateServiceProviderProfileHandler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger)
    : IRequestHandler<UpdateServiceProviderProfileCommand, ResponseModel<UpdateServiceProviderProfileDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<UpdateServiceProviderProfileDto>> Handle(UpdateServiceProviderProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByIdAsync(request.Dto.Id);
            if (profile == null)
                throw new NotFoundException("Service Provider Profile not found.");

            // dto deyerlerini yenileryirik burda hazirki entity ile 
            _mapper.Map(request.Dto, profile);
            profile.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.ServiceProviderProfileRepository.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();

            #region ActivityLog
            await _activityLogger.LogAsync(
                userId: profile.UserId,  
                action: "Update",
                entityType: "ServiceProviderProfile",
                entityId: profile.Id,  
                performedBy: request.Dto.UpdatedBy,  
                description: $"Service provider profile with Id {profile.Id} updated. " +
                             $"Full Name: {profile.Name}, City: {profile.City}, " +
                             $"Age: {profile.Age}, Experience: {profile.ExperienceYears}, " +
                             $"Description: {profile.Description}. Updated by: {request.Dto.UpdatedBy}."
            );
            #endregion



            // Yenilənmiş Entity-ni DTO-ya çeviririk
            var responseDto = _mapper.Map<UpdateServiceProviderProfileDto>(profile);

            return new ResponseModel<UpdateServiceProviderProfileDto>
            {
                Data = responseDto,
                IsSuccess = true,
                Errors = ["Service provider profile updated successfully."]
            };
        }


    }
}
