using Domain.BaseEntities;
using Domain.Types;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public int? RoleId { get; set; }
        public bool IsActive { get; set; } = true;

        public UserRole? Role { get; set; }

        public ClientProfile? ClientProfile { get; set; }
        public ServiceProviderProfile? ServiceProviderProfile { get; set; }
    }
}
