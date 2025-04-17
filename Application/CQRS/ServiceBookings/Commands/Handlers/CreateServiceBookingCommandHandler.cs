using Application.Common.Interfaces;
using Application.CQRS.Notifications.DTOs;
using Application.CQRS.ServiceBookings.DTOs;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Commands.Create;

public class CreateServiceBookingCommandHandler
    : IRequestHandler<CreateServiceBookingCommandRequest, ResponseModel<int>>
{
    private readonly IUserContext _userContext;
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public CreateServiceBookingCommandHandler(
        IUserContext userContext,
        IClientProfileRepository clientProfileRepository,
        IServiceBookingRepository serviceBookingRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _userContext = userContext;
        _clientProfileRepository = clientProfileRepository;
        _serviceBookingRepository = serviceBookingRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
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

        var serviceProviderProfile = await _unitOfWork.ServiceProviderProfileRepository
            .GetByIdAsync(dto.ServiceProviderProfileId);

        if (serviceProviderProfile is null)
            throw new NotFoundException("Service provider profile not found");

        var providerUserId = serviceProviderProfile.UserId;
        var clientUserId = userId;

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

        var nameSurname = await _clientProfileRepository.GetNameSurnameByUserIdAsync(clientUserId);
        var fullName = nameSurname != null
            ? $"{nameSurname.Value.Name} {nameSurname.Value.Surname}"
            : $"User {clientUserId}";

        await _notificationService.CreateAsync(new CreateNotificationDto
        {
            UserId = providerUserId,
            TypeId = NotificationTypeConstants.NewBooking,
            Message = $"You have a new booking from {fullName}!",
            CreatedBy = clientUserId
        });

        return ResponseModel<int>.Success(booking.Id);
    }
}
