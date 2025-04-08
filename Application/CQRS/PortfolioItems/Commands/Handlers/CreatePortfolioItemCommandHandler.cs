using Application.CQRS.PortfolioItems.Commands.Requests;
using Application.CQRS.PortfolioItems.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.PortfolioItems.Commands.Handlers;

public class CreatePortfolioItemCommandHandler : IRequestHandler<CreatePortfolioItemCommand, ResponseModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePortfolioItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<string>> Handle(CreatePortfolioItemCommand request, CancellationToken cancellationToken)
    {
        var serviceProviderProfile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(request.CurrentUserId);
        if (serviceProviderProfile == null)
            throw new NotFoundException("Service provider profile not found");

        var entity = new PortfolioItem
        {
            ServiceProviderProfileId = serviceProviderProfile.Id,
            Title = request.Dto.Title,
            Description = request.Dto.Description,
            ImageUrl = request.Dto.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.CurrentUserId
        };

        await _unitOfWork.PortfolioItemRepository.AddAsync(entity);

        return new ResponseModel<string>
        {
            Data = "Portfolio item created successfully",
            IsSuccess = true
        };
    }
}
