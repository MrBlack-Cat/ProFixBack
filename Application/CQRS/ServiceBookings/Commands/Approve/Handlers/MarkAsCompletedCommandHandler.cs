using Application.Common.Interfaces;
using Common.Exceptions;
using Domain.Enums;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Commands.Complete;

public class MarkAsCompletedCommandHandler : IRequestHandler<MarkAsCompletedCommandRequest>
{
    private readonly IUserContext _userContext;
    private readonly IServiceProviderProfileRepository _providerRepository;
    private readonly IServiceBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkAsCompletedCommandHandler(
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

    public async Task<Unit> Handle(MarkAsCompletedCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();

        var provider = await _providerRepository.GetByUserIdAsync(userId)
            ?? throw new NotFoundException("Service provider not found");

        var booking = await _bookingRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("Service booking not found");

        if (booking.ServiceProviderProfileId != provider.Id)
            throw new ForbiddenException("You can only complete your own bookings");

        if (!booking.IsConfirmedByProvider)
            throw new ConflictException("Cannot complete unconfirmed booking");

        if (booking.IsCompleted)
            throw new ConflictException("This booking is already completed");

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
