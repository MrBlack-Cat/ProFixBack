using Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Message : BaseEntity
{
    public int SenderUserId { get; set; }
    public int ReceiverUserId { get; set; }
    public string Content { get; set; } = null!;
    public bool IsRead { get; set; } = false;

    public User? Sender { get; set; }
    public User? Receiver { get; set; }
}
