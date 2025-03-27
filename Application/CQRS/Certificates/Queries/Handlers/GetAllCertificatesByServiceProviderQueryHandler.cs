using Application.CQRS.Certificates.DTOs;
using Application.CQRS.Certificates.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Certificates.Queries.Handlers;

public class GetAllCertificatesByServiceProviderQueryHandler : IRequestHandler<GetAllCertificatesByServiceProviderQuery, ResponseModel<List<CertificateListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCertificatesByServiceProviderQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<CertificateListDto>>> Handle(GetAllCertificatesByServiceProviderQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.CertificateRepository.GetByServiceProviderIdAsync(request.ServiceProviderProfileId);

        var filtered = list.Where(c => !c.IsDeleted).ToList();
        var result = _mapper.Map<List<CertificateListDto>>(filtered);

        return new ResponseModel<List<CertificateListDto>> { Data = result, IsSuccess = true };
    }
}
