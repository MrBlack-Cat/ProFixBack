using Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Review : BaseEntity
{
    public int ClientProfileId { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string? ClientName { get; set; }         
    public string? ClientAvatarUrl { get; set; }

    public ClientProfile? ClientProfile { get; set; }
    public ServiceProviderProfile? ServiceProviderProfile { get; set; }
}
