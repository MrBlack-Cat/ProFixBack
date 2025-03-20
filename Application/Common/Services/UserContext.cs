using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Common.Exceptions;

namespace Application.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId != null ? int.Parse(userId) : null;
        }

        public string? GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        public int MustGetUserId()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not authenticated.");
            return userId.Value;
        }
    }
}
