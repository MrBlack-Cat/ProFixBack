using Application.CQRS.Certificates.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class CertificateProfile : Profile
{
    public CertificateProfile()
    {
        CreateMap<Certificate, CreateCertificateDto>().ReverseMap();
        CreateMap<Certificate, UpdateCertificateDto>().ReverseMap();
        CreateMap<Certificate, GetCertificateByIdDto>().ReverseMap();
        CreateMap<Certificate, CertificateListDto>().ReverseMap();
    }
}
