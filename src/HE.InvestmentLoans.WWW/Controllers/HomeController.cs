using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace HE.InvestmentLoans.WWW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IMediator mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public IActionResult Index()
        {
            var x = this.User.Claims.ToList();
            var contactuserroles = mediator.Send(new BusinessLogic._LoanApplication.Commands.GetUserRolesFromCrm() { contactEmail = "aa2@aa.aa", contactExternalId = "{27B157CC-4BB0-4B76-B1F0-C6D8AE1BEAFA}" }).GetAwaiter().GetResult();


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
