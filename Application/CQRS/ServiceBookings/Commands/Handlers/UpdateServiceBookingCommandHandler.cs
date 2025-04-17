using Application.Common.Interfaces;
using Application.CQRS.ServiceBookings.DTOs;
using Common.Exceptions;
using Domain.Entities;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Commands.Update;

public class UpdateServiceBookingCommandHandler : IRequestHandler<UpdateServiceBookingCommandRequest>
{
    private readonly IUserContext _userContext;
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceBookingCommandHandler(
        IUserContext userContext,
        IServiceBookingRepository serviceBookingRepository,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _serviceBookingRepository = serviceBookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateServiceBookingCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();
        var booking = await _serviceBookingRepository.GetByIdAsync(request.Id);

        if (booking is null || booking.IsDeleted)
            throw new NotFoundException("Service booking not found");

        if (booking.ClientProfile?.UserId != userId)
            throw new ForbiddenException("You can only update your own bookings");

        if (booking.IsConfirmedByProvider)
            throw new ConflictException("You cannot update a confirmed booking");

        var dto = request.Dto;

        if (dto.ScheduledDate.HasValue && dto.ScheduledDate.Value.Date < DateTime.UtcNow.Date)
            throw new ValidationException(new List<string> { "Cannot update to a past date" });

        if (dto.StartTime.HasValue && dto.EndTime.HasValue && dto.EndTime <= dto.StartTime)
            throw new ValidationException(new List<string> { "End time must be after start time" });

        //vaxti yoxlama
        if (dto.ScheduledDate.HasValue || dto.StartTime.HasValue || dto.EndTime.HasValue)
        {
            var date = dto.ScheduledDate ?? booking.ScheduledDate;
            var start = dto.StartTime ?? booking.StartTime;
            var end = dto.EndTime ?? booking.EndTime;

            var isAvailable = await _serviceBookingRepository.IsTimeSlotAvailableAsync(
                booking.ServiceProviderProfileId, date, start, end
            );

            if (!isAvailable)
                throw new ConflictException("Selected time slot is already taken");
        }

        
        if (dto.Description != null)
            booking.Description = dto.Description;

        if (dto.ScheduledDate.HasValue)
            booking.ScheduledDate = dto.ScheduledDate.Value.Date;

        if (dto.StartTime.HasValue)
            booking.StartTime = dto.StartTime.Value;

        if (dto.EndTime.HasValue)
            booking.EndTime = dto.EndTime.Value;

        booking.UpdatedAt = DateTime.UtcNow;
        booking.UpdatedBy = userId;

        await _serviceBookingRepository.UpdateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
