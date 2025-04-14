using Application.Common.Interfaces;
using Common.Exceptions;
using Domain.Enums;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Commands.Approve;

public class ApproveServiceBookingCommandHandler : IRequestHandler<ApproveServiceBookingCommandRequest>
{
    private readonly IUserContext _userContext;
    private readonly IServiceProviderProfileRepository _providerRepository;
    private readonly IServiceBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveServiceBookingCommandHandler(
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

    public async Task<Unit> Handle(ApproveServiceBookingCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();

        var provider = await _providerRepository.GetByUserIdAsync(userId)
            ?? throw new NotFoundException("Service provider profile not found");

        var booking = await _bookingRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("Service booking not found");

        // 🔒 Проверка принадлежности
        if (booking.ServiceProviderProfileId != provider.Id)
            throw new ForbiddenException("You can only approve your own bookings");

        // ❗ Бизнес-логика: запрещаем одобрять отменённые, отклонённые или завершённые брони
        if (booking.StatusId == (int)ServiceBookingStatusEnum.Cancelled ||
            booking.StatusId == (int)ServiceBookingStatusEnum.Rejected ||
            booking.StatusId == (int)ServiceBookingStatusEnum.Completed)
        {
            throw new ConflictException("You cannot approve a cancelled, rejected or completed booking");
        }

        // ✅ Установка полей подтверждения
        booking.IsConfirmedByProvider = true;
        booking.ConfirmationDate = DateTime.UtcNow;
        booking.StatusId = (int)ServiceBookingStatusEnum.Approved;
        booking.Status = "Approved";
        booking.UpdatedAt = DateTime.UtcNow;
        booking.UpdatedBy = userId;

        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
