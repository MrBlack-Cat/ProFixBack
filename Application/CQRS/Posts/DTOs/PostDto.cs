﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Posts.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public int ServiceProviderProfileId { get; set; }
        public string ServiceProviderName { get; set; } = null!;
        public string ServiceProviderSurname { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikesCount { get; set; }
        public bool HasLiked { get; set; }

    }
}
