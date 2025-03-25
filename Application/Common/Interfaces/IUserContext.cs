using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface IUserContext
{
    int? GetCurrentUserId();
    string? GetCurrentUserName();
    int MustGetUserId();
    ClaimsPrincipal? GetCurrentUser();        
    string? GetUserRole();                
}
