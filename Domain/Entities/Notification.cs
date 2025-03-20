using Domain.BaseEntities;
using Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Notification : BaseEntity
{
    public int UserId { get; set; }
    public int TypeId { get; set; }
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; } = false;

    public User? User { get; set; }
    public NotificationType? Type { get; set; }
}
