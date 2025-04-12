using Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class GuaranteeDocument : BaseEntity
{
    public int ServiceProviderProfileId { get; set; }
    public int ClientProfileId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }

    public ServiceProviderProfile? ServiceProviderProfile { get; set; }
    public ClientProfile? ClientProfile { get; set; }
}
