using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface IGuaranteeDocumentRepository : IRepository<GuaranteeDocument>
    {
        Task<IEnumerable<GuaranteeDocument>> GetByClientIdAsync(int clientProfileId);
        Task<IEnumerable<GuaranteeDocument>> GetByServiceProviderIdAsync(int serviceProviderProfileId);
        Task<IEnumerable<GuaranteeDocument>> GetByUserIdAsync(int userId);

    }
}
