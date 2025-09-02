using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public interface INotificationService
{
    Task<IEnumerable<RefillNotificationDto>> GetRefillNotificationsAsync();
}
