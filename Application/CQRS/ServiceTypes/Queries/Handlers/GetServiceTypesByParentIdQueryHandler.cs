using Application.CQRS.ServiceTypes.DTOs;
using Application.CQRS.ServiceTypes.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.ServiceTypes.Queries.Handlers
{
    public class GetServiceTypesByParentIdQueryHandler
        : IRequestHandler<GetServiceTypesByParentIdQuery, ResponseModel<List<ServiceTypeListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetServiceTypesByParentIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel<List<ServiceTypeListDto>>> Handle(GetServiceTypesByParentIdQuery request, CancellationToken cancellationToken)
        {
            var all = await _unitOfWork.ServiceTypeRepository.GetAllAsync();
            var filtered = all.Where(x => x.ParentCategoryId == request.ParentCategoryId).ToList();
            var result = _mapper.Map<List<ServiceTypeListDto>>(filtered);

            return new ResponseModel<List<ServiceTypeListDto>> { Data = result, IsSuccess = true };
        }
    }

}
