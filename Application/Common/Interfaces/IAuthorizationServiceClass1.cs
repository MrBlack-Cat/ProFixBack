namespace Application.Common.Interfaces;

public interface IAuthorizationService
{
    void AuthorizeOwnerOrAdmin(int resourceOwnerId, int currentUserId, string currentUserRole);
    void AuthorizeRoles(string currentUserRole, params string[] allowedRoles);
}
