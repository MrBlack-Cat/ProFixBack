using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class GetAllRolesHandler
{
    public record struct Query : IRequest<ResponseModel<IEnumerable<UserRole>>>;

    public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Query, ResponseModel<IEnumerable<UserRole>>>
    {
        public async Task<ResponseModel<IEnumerable<UserRole>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var roles = await unitOfWork.UserRoleRepository.GetAllRolesAsync();
            return new ResponseModel<IEnumerable<UserRole>>
            {
                Data = roles,
                IsSuccess = true
            };
        }
    }
}
