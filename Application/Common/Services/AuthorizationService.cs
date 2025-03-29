using Application.Common.Interfaces;
using Common.Exceptions;

namespace Application.Services;

public class AuthorizationService : IAuthorizationService
{
    public void AuthorizeOwnerOrAdmin(int resourceOwnerId, int currentUserId, string currentUserRole)
    {
        if (currentUserId != resourceOwnerId && currentUserRole != "Admin")
        {
            throw new ForbiddenException("Access denied.");
        }
    }

    public void AuthorizeRoles(string currentUserRole, params string[] allowedRoles)
    {
        if (!allowedRoles.Contains(currentUserRole))
        {
            throw new ForbiddenException("You are not authorized to access this resource.");
        }
    }
}
