using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface ISupportTicketRepository : IRepository<SupportTicket>
    {
        Task<IEnumerable<SupportTicket>> GetByUserIdAsync(int userId);
    }
}
