using Application.Common.Interfaces;
using Common.Exceptions;
using Domain.Enums;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Commands.Complete;

public class CompleteServiceBookingCommandHandler : IRequestHandler<CompleteServiceBookingCommandRequest>
{
    private readonly IUserContext _userContext;
    private readonly IServiceProviderProfileRepository _providerRepository;
    private readonly IServiceBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteServiceBookingCommandHandler(
        IUserContext userContext,
        IServiceProviderProfileRepository providerRepository,
        IServiceBookingRepository bookingRepository,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _providerRepository = providerRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CompleteServiceBookingCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();

        var provider = await _providerRepository.GetByUserIdAsync(userId)
            ?? throw new NotFoundException("Service provider not found");

        var booking = await _bookingRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("Service booking not found");

        if (booking.ServiceProviderProfileId != provider.Id)
            throw new ForbiddenException("You can only complete your own bookings");

        if (booking.StatusId == (int)ServiceBookingStatusEnum.Completed)
            throw new ConflictException("Booking is already marked as completed");

        if (booking.StatusId == (int)ServiceBookingStatusEnum.Cancelled ||
            booking.StatusId == (int)ServiceBookingStatusEnum.Rejected)
            throw new ConflictException("Cannot complete a cancelled or rejected booking");

        booking.IsCompleted = true;
        booking.CompletionDate = DateTime.UtcNow;
        booking.StatusId = (int)ServiceBookingStatusEnum.Completed;
        booking.Status = "Completed";
        booking.UpdatedAt = DateTime.UtcNow;
        booking.UpdatedBy = userId;

        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
