using Application.CQRS.GuaranteeDocuments.DTOs;
using Application.CQRS.GuaranteeDocuments.Queries.Requests;
using Application.Common.Interfaces;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.GuaranteeDocuments.Queries.Handlers;

public class GetAllGuaranteeDocumentsQueryHandler : IRequestHandler<GetAllGuaranteeDocumentsQuery, ResponseModel<List<GuaranteeDocumentListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IAuthorizationService _authorizationService;

    public GetAllGuaranteeDocumentsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContext userContext,
        IAuthorizationService authorizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContext = userContext;
        _authorizationService = authorizationService;
    }

    public async Task<ResponseModel<List<GuaranteeDocumentListDto>>> Handle(GetAllGuaranteeDocumentsQuery request, CancellationToken cancellationToken)
    {
        var role = _userContext.GetUserRole();
        _authorizationService.AuthorizeRoles(role, "Admin");

        var docs = await _unitOfWork.GuaranteeDocumentRepository.GetAllAsync();
        var filtered = docs.Where(x => !x.IsDeleted).ToList();

        var result = _mapper.Map<List<GuaranteeDocumentListDto>>(filtered);
        return new ResponseModel<List<GuaranteeDocumentListDto>> { Data = result, IsSuccess = true };
    }
}
