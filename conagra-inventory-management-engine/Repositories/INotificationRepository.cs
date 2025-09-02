using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Repositories;

public interface INotificationRepository
{
    Task<IEnumerable<RefillNotificationData>> GetRefillNotificationsAsync();
}
