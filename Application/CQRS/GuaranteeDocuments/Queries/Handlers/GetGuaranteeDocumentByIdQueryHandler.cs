using Application.CQRS.GuaranteeDocuments.DTOs;
using Application.CQRS.GuaranteeDocuments.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.GuaranteeDocuments.Queries.Handlers;

public class GetGuaranteeDocumentByIdQueryHandler : IRequestHandler<GetGuaranteeDocumentByIdQuery, ResponseModel<GetGuaranteeDocumentByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGuaranteeDocumentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<GetGuaranteeDocumentByIdDto>> Handle(GetGuaranteeDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.GuaranteeDocumentRepository.GetByIdAsync(request.Id);
        if (entity == null || entity.IsDeleted)
            throw new NotFoundException("Guarantee document not found.");

        var dto = _mapper.Map<GetGuaranteeDocumentByIdDto>(entity);
        return new ResponseModel<GetGuaranteeDocumentByIdDto> { Data = dto, IsSuccess = true };
    }
}
