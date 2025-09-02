using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("refillNotification")]
    public async Task<ActionResult<IEnumerable<RefillNotificationDto>>> GetRefillNotifications()
    {
        try
        {
            var refillNotifications = await _notificationService.GetRefillNotificationsAsync();
            return Ok(refillNotifications);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
