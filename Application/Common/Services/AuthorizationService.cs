using Application.Common.Interfaces;
using Common.Exceptions;

namespace Application.Common.Services;

public class AuthorizationService : IAuthorizationService
{
    public void AuthorizeOwnerOrAdmin(int resourceOwnerId, int currentUserId, string currentUserRole)
    {
        if (resourceOwnerId != currentUserId && currentUserRole != "Admin")
        {
            throw new ForbiddenException("You do not have permission to access this resource.");
        }
    }
}
