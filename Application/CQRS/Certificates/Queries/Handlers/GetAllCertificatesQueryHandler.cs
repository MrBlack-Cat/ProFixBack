using Application.CQRS.Certificates.DTOs;
using Application.CQRS.Certificates.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Certificates.Queries.Handlers;

public class GetAllCertificatesQueryHandler : IRequestHandler<GetAllCertificatesQuery, ResponseModel<List<CertificateListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCertificatesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<CertificateListDto>>> Handle(GetAllCertificatesQuery request, CancellationToken cancellationToken)
    {
        var all = await _unitOfWork.CertificateRepository.GetAllAsync();
        var filtered = all.Where(x => !x.IsDeleted).ToList();

        var dtoList = _mapper.Map<List<CertificateListDto>>(filtered);

        return new ResponseModel<List<CertificateListDto>> { Data = dtoList, IsSuccess = true };
    }
}
