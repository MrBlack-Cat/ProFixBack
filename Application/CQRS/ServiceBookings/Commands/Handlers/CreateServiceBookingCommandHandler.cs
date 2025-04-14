using Application.Common.Interfaces;
using Application.CQRS.ServiceBookings.DTOs;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Repository.Common;
using Repository.Repositories;
using static Dapper.SqlMapper;

namespace Application.CQRS.ServiceBookings.Commands.Create;

public class CreateServiceBookingCommandHandler
    : IRequestHandler<CreateServiceBookingCommandRequest, ResponseModel<int>> // ✅
{
    private readonly IUserContext _userContext;
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceBookingCommandHandler(
        IUserContext userContext,
        IClientProfileRepository clientProfileRepository,
        IServiceBookingRepository serviceBookingRepository,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _clientProfileRepository = clientProfileRepository;
        _serviceBookingRepository = serviceBookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel<int>> Handle(CreateServiceBookingCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();

        var clientProfile = await _clientProfileRepository.GetByUserIdAsync(userId);
        if (clientProfile is null)
            throw new NotFoundException("Client profile not found");

        var dto = request.Dto;

        if (dto.ScheduledDate.Date < DateTime.UtcNow.Date)
            return ResponseModel<int>.Fail("Cannot create booking in the past");

        if (dto.EndTime <= dto.StartTime)
            return ResponseModel<int>.Fail("End time must be after start time");

        var isAvailable = await _serviceBookingRepository.IsTimeSlotAvailableAsync(
            dto.ServiceProviderProfileId, dto.ScheduledDate.Date, dto.StartTime, dto.EndTime
        );

        if (!isAvailable)
            return ResponseModel<int>.Fail("Selected time slot is already taken");

        var booking = new ServiceBooking
        {
            ClientProfileId = clientProfile.Id,
            ServiceProviderProfileId = dto.ServiceProviderProfileId,
            Description = dto.Description,
            ScheduledDate = dto.ScheduledDate.Date,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            StatusId = (int)ServiceBookingStatusEnum.Pending,
            Status = "Pending",
            IsConfirmedByProvider = false,
            IsCompleted = false,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _serviceBookingRepository.AddAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return ResponseModel<int>.Success(booking.Id);
    }
}
