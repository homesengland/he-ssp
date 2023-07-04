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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
