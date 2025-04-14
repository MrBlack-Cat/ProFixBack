using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ServiceBookingStatusEnum
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Cancelled = 4,
        InProgress = 5,
        Completed = 6
    }

}
