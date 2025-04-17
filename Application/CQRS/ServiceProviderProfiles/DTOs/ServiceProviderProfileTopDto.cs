using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.ServiceProviderProfiles.DTOs;

public class ServiceProviderProfileTopDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? City { get; set; }
    public int? Age { get; set; }
    public int? ExperienceYears { get; set; }
    public string? AvatarUrl { get; set; }
    public double AverageRating { get; set; } 
    public string? ParentCategoryName { get; set; } 
}
