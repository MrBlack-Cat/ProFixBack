using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface IPostLikeRepository : IRepository<PostLike>
    {
        Task<bool> HasLikedAsync(int postId, int clientProfileId);
        Task<int> GetLikesCountAsync(int postId);
        Task AddLikeAsync(int postId, int clientProfileId);
        Task RemoveLikeAsync(int postId, int clientProfileId);
        Task<IEnumerable<PostLike>> GetAllPostLikesAsync();
    }

}
