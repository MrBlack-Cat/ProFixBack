using Application.CQRS.ParentCategories.DTOs;
using Common.GlobalResponse;
using MediatR;

public record GetAllParentCategoriesQuery() : IRequest<ResponseModel<List<ParentCategoryListDto>>>;
