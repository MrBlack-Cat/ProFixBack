using Application.Common.Interfaces;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using System.Security.Claims;

namespace Application.CQRS.Users.Handlers;

public class DeleteHandler
{
    public record struct Command : IRequest<ResponseModel<Unit>>
    {
        public int Id { get; set; }
        public int DeletedBy { get; set; }
        public string DeletedReason { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<Command, ResponseModel<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserContext _userContext = userContext;

        public async Task<ResponseModel<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var currentUserId = _userContext.GetCurrentUserId();
            if (!currentUserId.HasValue)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var userRole = _userContext.GetUserRole();
            if (userRole != "Admin")
                throw new ForbiddenException("Only Admins can delete users.");

            if (string.IsNullOrWhiteSpace(request.DeletedReason))
                throw new BadRequestException("Delete reason is required.");

            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new BadRequestException("User does not exist with the provided ID.");

            user.IsDeleted = true;
            user.IsActive = false;
            user.DeletedAt = DateTime.UtcNow;
            user.DeletedBy = request.DeletedBy;
            user.DeletedReason = request.DeletedReason;

            await _unitOfWork.UserRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel<Unit>
            {
                Data = Unit.Value,
                IsSuccess = true
            };
        }
    }
}
