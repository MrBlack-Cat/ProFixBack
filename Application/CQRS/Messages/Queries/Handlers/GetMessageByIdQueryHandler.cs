using Application.CQRS.Messages.DTOs;
using Application.CQRS.Messages.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Messages.Queries.Handlers;

public class GetMessageByIdQueryHandler : IRequestHandler<GetMessageByIdQuery, ResponseModel<GetMessageByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMessageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<GetMessageByIdDto>> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
    {
        var message = await _unitOfWork.MessageRepository.GetByIdAsync(request.Id);
        if (message is null || message.IsDeleted)
            throw new NotFoundException("Message not found.");

        var dto = _mapper.Map<GetMessageByIdDto>(message);
        return new ResponseModel<GetMessageByIdDto> { Data = dto, IsSuccess = true };
    }
}
