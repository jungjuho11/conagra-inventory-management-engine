using conagra_inventory_management_engine.DTOs;
using conagra_inventory_management_engine.Repositories;

namespace conagra_inventory_management_engine.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<IEnumerable<RefillNotificationDto>> GetRefillNotificationsAsync()
    {
        var refillData = await _notificationRepository.GetRefillNotificationsAsync();
        
        return refillData.Select(data => new RefillNotificationDto
        {
            StoreId = data.StoreId,
            StoreName = data.StoreName,
            StoreAddress = data.StoreAddress,
            ProductId = data.ProductId,
            ProductName = data.ProductName,
            CurrentQuantity = data.CurrentQuantity,
            ThresholdQuantity = data.ThresholdQuantity,
            QuantityNeeded = data.QuantityNeeded,
            IsUrgent = data.IsUrgent
        });
    }
}
