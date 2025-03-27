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



    public sealed class UpdateServiceProviderProfileHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateServiceProviderProfileCommand, ResponseModel<UpdateServiceProviderProfileDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

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
