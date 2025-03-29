using Application.CQRS.GuaranteeDocuments.DTOs;
using Application.CQRS.GuaranteeDocuments.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.GuaranteeDocuments.Queries.Handlers;

public class GetAllGuaranteesByProviderIdHandler : IRequestHandler<GetAllGuaranteesByProviderIdQuery, ResponseModel<List<GuaranteeDocumentListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllGuaranteesByProviderIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<GuaranteeDocumentListDto>>> Handle(GetAllGuaranteesByProviderIdQuery request, CancellationToken cancellationToken)
    {
        var docs = await _unitOfWork.GuaranteeDocumentRepository.GetByServiceProviderIdAsync(request.ServiceProviderProfileId);
        var filtered = docs.Where(d => !d.IsDeleted).ToList();
        var result = _mapper.Map<List<GuaranteeDocumentListDto>>(filtered);

        return new ResponseModel<List<GuaranteeDocumentListDto>> { Data = result, IsSuccess = true };
    }
}
