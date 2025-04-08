namespace Application.CQRS.ServiceProviderProfiles.DTOs


{
    public class CreateServiceProviderProfileDto
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Age { get; set; }
        public int GenderId { get; set; } // 1 = male, 2 = female
        public int ExperienceYears { get; set; }
        //public string Description { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int ParentCategoryId { get; set; }

        public List<int> ServiceTypeIds { get; set; } = new();
    }
}