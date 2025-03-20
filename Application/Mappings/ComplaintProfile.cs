using Application.CQRS.Complaints.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ComplaintProfile : Profile
{
    public ComplaintProfile()
    {
        CreateMap<Complaint, CreateComplaintDto>().ReverseMap();
        CreateMap<Complaint, UpdateComplaintDto>().ReverseMap();
        CreateMap<Complaint, GetComplaintByIdDto>().ReverseMap();
        CreateMap<Complaint, ComplaintListDto>().ReverseMap();
    }
}
