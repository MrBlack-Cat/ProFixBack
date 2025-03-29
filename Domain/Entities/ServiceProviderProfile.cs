using Domain.BaseEntities;
using Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class ServiceProviderProfile : BaseEntity
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? City { get; set; }
    public int? Age { get; set; }
    public int? GenderId { get; set; }
    public int? ExperienceYears { get; set; }
    public string? Description { get; set; }
    public bool IsApprovedByAdmin { get; set; } = false;
    public DateTime? ApprovalDate { get; set; }

    public User? User { get; set; }
    public GenderType? Gender { get; set; }
}
