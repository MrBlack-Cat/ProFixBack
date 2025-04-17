using Application.Common.Interfaces;
using Common.Exceptions;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Commands.Delete;

public class DeleteServiceBookingCommandHandler : IRequestHandler<DeleteServiceBookingCommandRequest>
{
    private readonly IUserContext _userContext;
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceBookingCommandHandler(
        IUserContext userContext,
        IServiceBookingRepository serviceBookingRepository,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _serviceBookingRepository = serviceBookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteServiceBookingCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();
        var booking = await _serviceBookingRepository.GetByIdAsync(request.Id);

        if (booking is null || booking.IsDeleted)
            throw new NotFoundException("Service booking not found");

        // yalniz klient sile biler
        if (booking.ClientProfile?.UserId != userId)
            throw new ForbiddenException("You can only delete your own bookings");

        if (booking.IsConfirmedByProvider)
            throw new ConflictException("You cannot delete a confirmed booking");

        booking.IsDeleted = true;
        booking.DeletedAt = DateTime.UtcNow;
        booking.DeletedBy = userId;
        booking.DeletedReason = request.DeletedReason;

        await _serviceBookingRepository.UpdateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
