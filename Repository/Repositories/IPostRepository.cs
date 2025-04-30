using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Repository.Repositories;

public interface IPostRepository : IRepository<Post>
{
    Task<IEnumerable<Post>> GetByServiceProviderIdAsync(int serviceProviderProfileId);
    Task<IEnumerable<Post>> GetPostsByProviderIdAsync(int serviceProviderProfileId);
    Task<IEnumerable<Post>> GetPostsByLikedAsync();
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
    Task<int> GetTotalLikesByUserIdAsync(int userId);
    Task<IEnumerable<Post>> GetAllPostsAsync();


}
