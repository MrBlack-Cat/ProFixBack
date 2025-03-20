namespace Application.Common.Interfaces;

public interface IUserContext
{
    int? GetCurrentUserId();
    string? GetCurrentUserName();
    int MustGetUserId(); 
}
