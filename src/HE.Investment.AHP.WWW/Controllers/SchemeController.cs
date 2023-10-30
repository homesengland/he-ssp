using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Authorize]
[Route("scheme")]
public class SchemeController : Controller
{
}
