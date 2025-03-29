using Application.CQRS.Certificates.DTOs;
using Application.CQRS.Certificates.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Certificates.Queries.Handlers;

public class GetCertificateByIdQueryHandler : IRequestHandler<GetCertificateByIdQuery, ResponseModel<GetCertificateByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCertificateByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<GetCertificateByIdDto>> Handle(GetCertificateByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CertificateRepository.GetByIdAsync(request.Id);
        if (entity is null || entity.IsDeleted)
            throw new NotFoundException("Certificate not found.");

        var dto = _mapper.Map<GetCertificateByIdDto>(entity);
        return new ResponseModel<GetCertificateByIdDto> { Data = dto, IsSuccess = true };
    }
}
