using Application.CQRS.GuaranteeDocuments.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class GuaranteeDocumentProfile : Profile
{
    public GuaranteeDocumentProfile()
    {
        CreateMap<GuaranteeDocument, CreateGuaranteeDocumentDto>().ReverseMap();
        CreateMap<GuaranteeDocument, UpdateGuaranteeDocumentDto>().ReverseMap();
        CreateMap<GuaranteeDocument, GetGuaranteeDocumentByIdDto>().ReverseMap();
        CreateMap<GuaranteeDocument, GuaranteeDocumentListDto>().ReverseMap();

    }
}
