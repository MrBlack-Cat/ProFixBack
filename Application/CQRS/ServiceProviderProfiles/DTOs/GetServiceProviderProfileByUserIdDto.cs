using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.ServiceProviderProfiles.DTOs
{
    public class GetServiceProviderProfileByUserIdDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? City { get; set; }
        public int? Age { get; set; }
        public int? GenderId { get; set; }
        public string? GenderName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ExperienceYears { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Description { get; set; }
        public bool IsApprovedByAdmin { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public List<int> ServiceTypeIds { get; set; } = new();
        public List<string> ServiceTypes { get; set; } = new();
    }


}
