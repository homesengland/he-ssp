using HE.UtilsService.BannerNotification.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.UtilsService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BannerNotificationController : Controller
{
    [HttpPost("publish")]
    public IActionResult Publish(BannerNotificaiton request)
    {
        return Ok(request);
    }
}
