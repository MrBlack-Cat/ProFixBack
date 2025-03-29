using Application.CQRS.GuaranteeDocuments.DTOs;
using Application.CQRS.GuaranteeDocuments.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

public class GetAllGuaranteesByClientIdHandler : IRequestHandler<GetAllGuaranteesByClientIdQuery, ResponseModel<List<GuaranteeDocumentListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllGuaranteesByClientIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<GuaranteeDocumentListDto>>> Handle(GetAllGuaranteesByClientIdQuery request, CancellationToken cancellationToken)
    {
        var docs = await _unitOfWork.GuaranteeDocumentRepository.GetByClientIdAsync(request.ClientProfileId);
        var filtered = docs.Where(d => !d.IsDeleted).ToList();
        var result = _mapper.Map<List<GuaranteeDocumentListDto>>(filtered);

        return new ResponseModel<List<GuaranteeDocumentListDto>> { Data = result, IsSuccess = true };
    }
}
