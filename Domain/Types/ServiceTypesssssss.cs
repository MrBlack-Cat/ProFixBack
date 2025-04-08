using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Types;

public class ServiceTypesssssss
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }

    public ServiceTypesssssss? ParentCategory { get; set; }
}
