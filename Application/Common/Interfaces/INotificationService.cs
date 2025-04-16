using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface INotificationService
{
    Task CreateAsync(int receiverUserId, int typeId, string message, int createdBy);
}
