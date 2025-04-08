using Application.CQRS.Posts.DTOs;
using Application.CQRS.Posts.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Posts.Queries.Handlers;

public class GetPostsByProviderIdHandler : IRequestHandler<GetPostsByProviderIdQuery, ResponseModel<List<PostDto>>>
{
    private readonly IPostRepository _postRepository;
    private readonly IServiceProviderProfileRepository _serviceProviderProfileRepository;

    public GetPostsByProviderIdHandler(IPostRepository postRepository, IServiceProviderProfileRepository serviceProviderProfileRepository)
    {
        _postRepository = postRepository;
        _serviceProviderProfileRepository = serviceProviderProfileRepository;
    }

    public async Task<ResponseModel<List<PostDto>>> Handle(GetPostsByProviderIdQuery request, CancellationToken cancellationToken)
    {
        //var profile = await _serviceProviderProfileRepository.GetByUserIdAsync(request.ServiceProviderProfileId); // ⚠ тут userId
        //if (profile == null)
        //{
        //    return ResponseModel<List<PostDto>>.Fail("Service provider profile not found.");
        //}

        var posts = await _postRepository.GetPostsByProviderIdAsync(request.ServiceProviderProfileId);

        var result = posts.Select(p => new PostDto
        {
            Id = p.Id,
            ServiceProviderProfileId = p.ServiceProviderProfileId,
            Title = p.Title,
            Content = p.Content,
            ImageUrl = p.ImageUrl,
            CreatedAt = p.CreatedAt,
            CreatedBy = p.CreatedBy?.ToString()
        }).ToList();

        return ResponseModel<List<PostDto>>.Success(result);
    }
}
