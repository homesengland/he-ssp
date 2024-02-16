using HE.Investments.Account.Shared.Routing;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly AccountConfig _accountConfig;

    public AccountController(AccountConfig accountConfig)
    {
        _accountConfig = accountConfig;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return new RedirectResult(_accountConfig.Url);
    }
}
