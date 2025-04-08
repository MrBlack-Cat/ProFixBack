//using Application.CQRS.Posts.DTOs;
//using Application.CQRS.Posts.Queries.Requests;
//using Common.GlobalResponse;
//using AutoMapper;
//using MediatR;
//using Repository.Repositories;

//namespace Application.CQRS.Posts.Handlers;

//public class GetPostsByProviderHandler : IRequestHandler<GetPostsByProviderQuery, ResponseModel<List<PostListDto>>>
//{
//    private readonly IPostRepository _postRepository;
//    private readonly IMapper _mapper;

//    public GetPostsByProviderHandler(IPostRepository postRepository, IMapper mapper)
//    {
//        _postRepository = postRepository;
//        _mapper = mapper;
//    }

//    public async Task<ResponseModel<List<PostListDto>>> Handle(GetPostsByProviderQuery request, CancellationToken cancellationToken)
//    {
//        var posts = await _postRepository.GetPostsByProviderIdAsync(request.UserId);
//        var dto = _mapper.Map<List<PostListDto>>(posts);

//        return ResponseModel<List<PostListDto>>.Success(dto);
//    }
//}

using Application.CQRS.Posts.DTOs;
using Application.CQRS.Posts.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Posts.Queries.Handlers;

public class GetPostsByProviderHandler : IRequestHandler<GetPostsByProviderQuery, ResponseModel<List<PostListDto>>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly IServiceProviderProfileRepository _profileRepository;

    public GetPostsByProviderHandler(
        IPostRepository postRepository,
        IMapper mapper,
        IServiceProviderProfileRepository profileRepository)
    {
        _postRepository = postRepository;
        _mapper = mapper;
        _profileRepository = profileRepository;
    }

    public async Task<ResponseModel<List<PostListDto>>> Handle(GetPostsByProviderQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByUserIdAsync(request.UserId);
        if (profile == null)
            return ResponseModel<List<PostListDto>>.Fail("Service provider profile not found.");

        var posts = await _postRepository.GetPostsByProviderIdAsync(profile.Id);
        var dto = _mapper.Map<List<PostListDto>>(posts);

        return ResponseModel<List<PostListDto>>.Success(dto);
    }
}
