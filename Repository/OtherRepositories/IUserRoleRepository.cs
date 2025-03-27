using Domain.Entities;

public interface IUserRoleRepository
{
    Task<IEnumerable<UserRole>> GetAllRolesAsync();
}
