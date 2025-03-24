using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Users.Handlers;

public class DeleteHandler
{
    public record struct Command : IRequest<ResponseModel<Unit>>
    {
        public int Id { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Command, ResponseModel<Unit>>
    {

        public readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseModel<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {

            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (currentUser == null) 
            {
                throw new BadRequestException("User does not exist with provided Id");
            }

            _unitOfWork.UserRepository.DeleteAsync(currentUser);

            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel<Unit>
            {
                Data = Unit.Value,
                Errors = [],
                IsSuccess = true,

            };



        }
    }
}
