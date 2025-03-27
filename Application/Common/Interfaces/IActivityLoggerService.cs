namespace Application.Common.Interfaces
{
    public interface IActivityLoggerService
    {
        // General
        Task LogAsync(
            int userId,
            string action,
            string entityType,
            int entityId,
            int? performedBy = null,
            string? description = null);

        Task LogAsync(
            int userId,
            string action,
            string entityType,
            int? performedBy = null,
            string? description = null);

        // System
        Task LogAsync(
            string action,
            string entityType,
            string? description = null);
    }
}
