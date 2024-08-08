using HE.UtilsService.BannerNotification;
using Microsoft.AspNetCore.Mvc;

namespace HE.UtilsService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BannerNotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public BannerNotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("publish")]
    public async Task<IActionResult> Publish(BannerNotification.Contract.BannerNotification request, CancellationToken cancellationToken)
    {
        await _notificationService.PublishNotification(request, cancellationToken);
        return Accepted();
    }
}
