using Domain.Types;

namespace Repository.Repositories;

public interface IComplaintTypeRepository
{
    Task<IEnumerable<ComplaintType>> GetAllAsync();
}
