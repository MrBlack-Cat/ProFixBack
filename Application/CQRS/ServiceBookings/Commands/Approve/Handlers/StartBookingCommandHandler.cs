// StartBookingCommandHandler.cs
using Application.Common.Interfaces;
using Common.Exceptions;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Commands.Start;
public class StartBookingCommandHandler : IRequestHandler<StartBookingCommandRequest>
{
    private readonly IUserContext _userContext;
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartBookingCommandHandler(
        IUserContext userContext,
        IServiceBookingRepository serviceBookingRepository,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _serviceBookingRepository = serviceBookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(StartBookingCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();
        var booking = await _serviceBookingRepository.GetByIdAsync(request.Id);
        if (booking == null || booking.IsDeleted)
            throw new NotFoundException("Service booking not found");

        // Tesdiqlenmish booking InProgress statusuna kece biler
        if (!booking.IsConfirmedByProvider)
            throw new ConflictException("Booking must be confirmed before starting");

        booking.StatusId = (int)ServiceBookingStatusEnum.InProgress;
        booking.UpdatedAt = DateTime.UtcNow;
        booking.UpdatedBy = userId;

        await _serviceBookingRepository.UpdateAsync(booking);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
