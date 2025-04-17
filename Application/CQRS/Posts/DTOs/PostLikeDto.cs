using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Posts.DTOs
{
    public class PostLikeDto
    {
        public int PostId { get; set; }
        public int ClientProfileId { get; set; }
    }

}
