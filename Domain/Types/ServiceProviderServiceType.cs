using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Types;

public class ServiceProviderServiceType
{
    public int Id { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public int ServiceTypeId { get; set; }

    public ServiceProviderProfile? ServiceProviderProfile { get; set; }
    public ServiceTypesssssss? ServiceType { get; set; }
}
