using Domain.BaseEntities;
using Domain.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SupportTicket : BaseEntity
    {
        public int UserId { get; set; }
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
        public int StatusId { get; set; }

        public User? User { get; set; }
        public SupportTicketStatus? Status { get; set; }
    }
}
