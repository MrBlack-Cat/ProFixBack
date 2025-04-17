using Domain.BaseEntities;
using Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Complaint : BaseEntity
{
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public int TypeId { get; set; }
    public string? Description { get; set; }

    public User? FromUser { get; set; }
    public User? ToUser { get; set; }
    public ComplaintType? Type { get; set; }
    public bool IsViewed { get; set; } = false;  
    public bool IsResolved { get; set; } = false; 
}
